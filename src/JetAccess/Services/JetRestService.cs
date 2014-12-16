using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using Jet.Misc;
using JetAccess.Misc;
using JetAccess.Models;
using JetAccess.Models.Services.JetRestService.GetOrderIds;
using JetAccess.Models.Services.JetRestService.GetOrderWithOutShipmentDetail;
using JetAccess.Models.Services.JetRestService.GetToken;
using JetAccess.Services.Parsers;

namespace JetAccess.Services
{
    internal class JetRestService: ICreateCallInfo
    {
        protected JetUserCredentials _userCredentials;
        protected TokenInfo _token;
        protected EndPoint _endPoint;
        public IWebRequestServices WebRequestServices{ get; set; }

        public JetRestService( JetUserCredentials userCredentials, EndPoint endPoint )
        {
            _userCredentials = userCredentials;
            _endPoint = endPoint;
            WebRequestServices = new WebRequestServices();
        }

        public async Task< TokenInfo > GetTokenOrReturnChachedAsync()
        {
            if( _token == null )
            {
                var res = await GetTokenAsync().ConfigureAwait( false );
                _token = res.TokenInfo;
            }

            return _token;
        }

        public async Task< GetTokenResponse > GetTokenAsync()
        {
            var mark = Guid.NewGuid().ToString();
            var body = string.Format( "{{\"user\":\"{0}\",\"pass\":\"{1}\"}}", _userCredentials.ApiUser, _userCredentials.Secret );
            var result = await InvokeCallAsync< GetTokenResponseParser, GetTokenResponse >( _endPoint.EndPointUrl + "/token/", RequestType.POST, mark, body ).ConfigureAwait( false );
            return result;
        }

        public async Task< GetOrderUrlsResponse > GetOrderUrlsAsync()
        {
            var mark = Guid.NewGuid().ToString();
            var token = await GetTokenOrReturnChachedAsync().ConfigureAwait( false );
            var header = new Dictionary< string, string >() { { "Authorization", token.ToString() } };
            var result = await InvokeCallAsync< GetOrderUrlsResponseParser, GetOrderUrlsResponse >( _endPoint.EndPointUrl + "/orders/ready?", RequestType.GET, mark, rawHeaders : header ).ConfigureAwait( false );
            return result;
        }

        public async Task< GetOrderWithoutShipmentDetailResponse > GetOrderWithoutShipmentDetailAsync( string orderUrl )
        {
            Condition.Requires( orderUrl ).IsNotNullOrWhiteSpace();

            var mark = Guid.NewGuid().ToString();
            var token = await GetTokenOrReturnChachedAsync().ConfigureAwait( false );
            var header = new Dictionary< string, string >() { { "Authorization", token.ToString() } };
            var result = await InvokeCallAsync< GetOrderWithoutShipmentDetailResponseParser, GetOrderWithoutShipmentDetailResponse >( _endPoint.EndPointUrl + orderUrl, RequestType.GET, mark, rawHeaders : header ).ConfigureAwait( false );
            return result;
        }

        protected sealed class RequestType
        {
            public static RequestType GET = new RequestType( "GET" );
            public static RequestType POST = new RequestType( "POST" );

            public String Type{ get; private set; }

            private RequestType( string type )
            {
                Type = type;
            }
        }

        protected async Task< TParsed > InvokeCallAsync< TParser, TParsed >( string partialUrl, RequestType requestType, string mark, string body = null, Dictionary< string, string > rawHeaders = null ) where TParser : IResponseParser< TParsed >, new()
        {
            var res = default( TParsed );
            try
            {
                await ActionPolicies.GetAsync.Do( async () =>
                {
                    WebRequest webRequest;
                    if( requestType == RequestType.POST )
                        webRequest = await WebRequestServices.CreatePostRequestAsync( partialUrl, body, rawHeaders ).ConfigureAwait( false );
                    else
                        webRequest = await WebRequestServices.CreateGetRequestAsync( partialUrl, body, rawHeaders ).ConfigureAwait( false );

                    using( var memStream = await this.WebRequestServices.GetResponseStreamAsync( webRequest ).ConfigureAwait( false ) )
                        res = new TParser().Parse( memStream, false );
                } ).ConfigureAwait( false );

                return res;
            }
            catch( Exception exception )
            {
                var parameters = string.Format( "{{Url:{0}, Body:{1}, Headers:{2}}}", partialUrl, body ?? PredefinedValues.NotAvailable, rawHeaders.ToJson() );
                throw new Exception( string.Format( "Exception occured. {0}", this.CreateMethodCallInfo( parameters, mark ) ), exception );
            }
        }
    }

    internal sealed class EndPoint
    {
        public static EndPoint Test = new EndPoint( "https://merchant-api.test.jet.com/api" );
        public static EndPoint Production = new EndPoint( "https://merchant-api.jet.com/api" );

        public String EndPointUrl{ get; private set; }

        private EndPoint( string endPointUrl )
        {
            EndPointUrl = endPointUrl;
        }
    }
}