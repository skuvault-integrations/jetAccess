using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jet.Misc;
using JetAccess.Misc;
using JetAccess.Models;
using JetAccess.Models.Services.JetRestService.GetToken;
using JetAccess.Services.Parsers;

namespace JetAccess.Services
{
    internal class JetRestService: ICreateCallInfo
    {
        protected JetUserCredentials _userCredentials;
        public IWebRequestServices WebRequestServices{ get; set; }

        public JetRestService( JetUserCredentials userCredentials )
        {
            _userCredentials = userCredentials;
            WebRequestServices = new WebRequestServices();
        }

        public async Task< GetTokenResponse > GetTokenAsync()
        {
            var mark = Guid.NewGuid().ToString();
            var body = string.Format( "{{\"user\":\"{0}\",\"pass\":\"{1}\"}}", _userCredentials.ApiUser, _userCredentials.Secret );
            var result = await InvokeCallAsync< GetTokenResponseParser, GetTokenResponse >( "https://merchant-api.test.jet.com/api/token/", mark, body ).ConfigureAwait( false );
            return result;
        }

        protected async Task< TParsed > InvokeCallAsync< TParser, TParsed >( string partialUrl, string mark, string body = null, Dictionary< string, string > rawHeaders = null ) where TParser : IResponseParser< TParsed >, new()
        {
            var res = default( TParsed );
            try
            {
                await ActionPolicies.GetAsync.Do( async () =>
                {
                    var webRequest = await WebRequestServices.CreatePostRequestAsync( partialUrl, body, rawHeaders ).ConfigureAwait( false );
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
}