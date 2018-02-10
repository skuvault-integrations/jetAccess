using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using Jet.Misc;
using JetAccess.Misc;
using JetAccess.Models;
using JetAccess.Models.Services.JetRestService.GetMerchantSkusInventory;
using JetAccess.Models.Services.JetRestService.GetOrderIds;
using JetAccess.Models.Services.JetRestService.GetOrderWithOutShipmentDetail;
using JetAccess.Models.Services.JetRestService.GetOrderWithShipmentDetail;
using JetAccess.Models.Services.JetRestService.GetProductUrls;
using JetAccess.Models.Services.JetRestService.GetToken;
using JetAccess.Models.Services.JetRestService.PutMerchantSkusInventory;
using JetAccess.Services.Parsers;

namespace JetAccess.Services
{
	internal class JetRestService: ICreateCallInfo, IJetRestService
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

		public GetTokenResponse GetToken()
		{
			var mark = Guid.NewGuid().ToString();
			var body = string.Format( "{{\"user\":\"{0}\",\"pass\":\"{1}\"}}", _userCredentials.ApiUser, _userCredentials.Secret );
			var result = InvokeCall< GetTokenResponseParser, GetTokenResponse >( _endPoint.EndPointUrl + "/token/", RequestType.POST, mark, body );
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

		public async Task< GetProductUrlsResponse > GetProductUrlsAsync()
		{
			var mark = Guid.NewGuid().ToString();
			var token = await GetTokenOrReturnChachedAsync().ConfigureAwait( false );
			var header = new Dictionary< string, string >() { { "Authorization", token.ToString() } };
			var result = await InvokeCallAsync< GetProductUrlsResponseParser, GetProductUrlsResponse >( _endPoint.EndPointUrl + "/merchant-skus?", RequestType.GET, mark, rawHeaders : header ).ConfigureAwait( false );
			return result;
		}

		public async Task< GetMerchantSkusInventoryResponse > GetMerchantSkusInventoryAsync( string productUrl )
		{
			Condition.Requires( productUrl ).IsNotNullOrWhiteSpace();

			var mark = Guid.NewGuid().ToString();
			var token = await GetTokenOrReturnChachedAsync().ConfigureAwait( false );
			var header = new Dictionary< string, string >() { { "Authorization", token.ToString() } };
			var result = await InvokeCallAsync< GetMerchantSkusInventoryResponseParser, GetMerchantSkusInventoryResponse >( _endPoint.EndPointUrl + "/" + productUrl + "/inventory?", RequestType.GET, mark, rawHeaders : header, returnDefaultInsteadOfException : true ).ConfigureAwait( false );
			return result;
		}

		public async Task< PatchMerchantSkusInventoryResponse > PatchMerchantSkusInventoryAsync( PatchMerchantSkusInventoryRequest putMerchantSkusInventoryRequest )
		{
			var mark = Guid.NewGuid().ToString();
			var token = await this.GetTokenOrReturnChachedAsync().ConfigureAwait( false );
			var header = new Dictionary< string, string > { { "Authorization", token.ToString() } };
			var body2 = putMerchantSkusInventoryRequest.ToJson();
			var result = await this.InvokeCallAsync< PatchMerchantSkusInventoryResponseParser, PatchMerchantSkusInventoryResponse >( this._endPoint.EndPointUrl + "/" + "merchant-skus" + "/" + putMerchantSkusInventoryRequest.Id + "/inventory?", RequestType.PATCH, mark, body2, header ).ConfigureAwait( false );
			return result;
		}

		public async Task< GetOrderWithShipmentDetailResponse > GetOrderWithShipmentDetailAsync( string orderUrl )
		{
			Condition.Requires( orderUrl ).IsNotNullOrWhiteSpace();

			var mark = Guid.NewGuid().ToString();
			var token = await GetTokenOrReturnChachedAsync().ConfigureAwait( false );
			var header = new Dictionary< string, string >() { { "Authorization", token.ToString() } };
			var result = await InvokeCallAsync< GetOrderWithShipmentDetailResponseParser, GetOrderWithShipmentDetailResponse >( _endPoint.EndPointUrl + orderUrl, RequestType.GET, mark, rawHeaders : header ).ConfigureAwait( false );
			return result;
		}

		protected sealed class RequestType
		{
			public static RequestType GET = new RequestType( "GET" );
			public static RequestType POST = new RequestType( "POST" );
			public static RequestType PUT = new RequestType( "PUT" );
			public static RequestType PATCH = new RequestType( "PATCH" );

			public String Type{ get; private set; }

			private RequestType( string type )
			{
				Type = type;
			}
		}

		protected TParsed InvokeCall< TParser, TParsed >( string partialUrl, RequestType requestType, string mark, string body = null, Dictionary< string, string > rawHeaders = null, bool returnDefaultInsteadOfException = false ) where TParser : class, IResponseParser< TParsed >, new()
		{
			var res = default( TParsed );
			try
			{
				ActionPolicies.Get.Do( () =>
				{
					WebRequest webRequest;
					if( requestType == RequestType.POST )
						webRequest = WebRequestServices.CreatePostRequest( partialUrl, body, rawHeaders );
					else if( requestType == RequestType.GET )
						webRequest = WebRequestServices.CreateGetRequest( partialUrl, body, rawHeaders );
					else if( requestType == RequestType.PUT )
						webRequest = WebRequestServices.CreatePutRequest( partialUrl, body, rawHeaders );
					else if( requestType == RequestType.PATCH )
						webRequest = this.WebRequestServices.CreatePatchRequest( partialUrl, body, rawHeaders );
					else
						webRequest = WebRequestServices.CreateGetRequest( partialUrl, body, rawHeaders );

					using( var memStream = this.WebRequestServices.GetResponseStream( webRequest ) )
						res = new TParser().Parse( memStream, false );
				} );

				return res;
			}
			catch( Exception exception )
			{
				if( returnDefaultInsteadOfException )
					return default( TParsed );
				var parameters = string.Format( "{{Url:{0}, Body:{1}, Headers:{2}}}", partialUrl, body ?? PredefinedValues.NotAvailable, rawHeaders.ToJson() );
				throw new Exception( string.Format( "Exception occured. {0}", this.CreateMethodCallInfo( parameters, mark ) ), exception );
			}
		}

		protected async Task< TParsed > InvokeCallAsync< TParser, TParsed >( string partialUrl, RequestType requestType, string mark, string body = null, Dictionary< string, string > rawHeaders = null, bool returnDefaultInsteadOfException = false ) where TParser : class, IResponseParser< TParsed >, new()
		{
			var res = default( TParsed );
			try
			{
				await ActionPolicies.GetAsync.Do( async () =>
				{
					WebRequest webRequest;
					if( requestType == RequestType.POST )
						webRequest = await WebRequestServices.CreatePostRequestAsync( partialUrl, body, rawHeaders ).ConfigureAwait( false );
					else if( requestType == RequestType.GET )
						webRequest = await WebRequestServices.CreateGetRequestAsync( partialUrl, body, rawHeaders ).ConfigureAwait( false );
					else if( requestType == RequestType.PUT )
						webRequest = await WebRequestServices.CreatePutRequestAsync( partialUrl, body, rawHeaders ).ConfigureAwait( false );
					else if( requestType == RequestType.PATCH )
						webRequest = await this.WebRequestServices.CreatePatchRequestAsync( partialUrl, body, rawHeaders ).ConfigureAwait( false );
					else
						webRequest = await WebRequestServices.CreateGetRequestAsync( partialUrl, body, rawHeaders ).ConfigureAwait( false );

					using( var memStream = await this.WebRequestServices.GetResponseStreamAsync( webRequest ).ConfigureAwait( false ) )
						res = new TParser().Parse( memStream, false );
				} ).ConfigureAwait( false );

				return res;
			}
			catch( Exception exception )
			{
				if( returnDefaultInsteadOfException )
					return default( TParsed );
				var parameters = string.Format( "{{Url:{0}, Body:{1}, Headers:{2}}}", partialUrl, body ?? PredefinedValues.NotAvailable, rawHeaders.ToJson() );
				throw new Exception( string.Format( "Exception occured. {0}", this.CreateMethodCallInfo( parameters, mark ) ), exception );
			}
		}
	}

	public sealed class EndPoint
	{
		public static EndPoint Test = new EndPoint( "https://merchant-api.jet.com/api" );
		public static EndPoint Production = new EndPoint( "https://merchant-api.jet.com/api" );

		public String EndPointUrl{ get; private set; }

		private EndPoint( string endPointUrl )
		{
			EndPointUrl = endPointUrl;
		}
	}
}