using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jet.Misc;
using JetAccess.Misc;
using JetAccess.Models;
using JetAccess.Models.GetOrders;
using JetAccess.Models.GetProducts;
using JetAccess.Models.Ping;
using JetAccess.Models.Services.JetRestService.PutMerchantSkusInventory;
using JetAccess.Models.UpdateInventory;
using JetAccess.Services;
using Netco.Extensions;

namespace JetAccess
{
	public class JetService: IJetService, ICreateCallInfo
	{
		private readonly int _batchSize = 16;
		internal IJetRestService JetRestService{ get; set; }
		public JetUserCredentials JetUserCredentials{ get; set; }
		public EndPoint EndPoint{ get; set; }

		public JetService( JetUserCredentials jetUserCredentials, EndPoint endPoint )
		{
			JetUserCredentials = jetUserCredentials;
			EndPoint = endPoint;
			JetRestService = new JetRestService( jetUserCredentials, EndPoint );
		}

		public Func< string > AdditionalLogInfo{ get; set; }

		public async Task< PingInfo > Ping()
		{
			try
			{
				var getTokenResponse = await JetRestService.GetTokenAsync().ConfigureAwait( false );
				var pingInfo = new PingInfo( !string.IsNullOrWhiteSpace( getTokenResponse.TokenInfo.Token ) && !string.IsNullOrWhiteSpace( getTokenResponse.TokenInfo.TokenType ) );
				return pingInfo;
			}
			catch( Exception exception )
			{
				var jetException = new JetException( this.CreateMethodCallInfo(), exception );
				JetLogger.LogTraceException( jetException );
				throw jetException;
			}
		}

		#region Inventory
		public async Task< IEnumerable< Product > > GetProductsAsync()
		{
			string methodParameters = string.Format( "{{{0}}}", PredefinedValues.NotAvailable );
			string mark = Guid.NewGuid().ToString();
			try
			{
				JetLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );
				var productUrls = await JetRestService.GetProductUrlsAsync().ConfigureAwait( false );

				var products = await productUrls.SkuUrls.ProcessInBatchAsync( _batchSize, async ( x ) =>
				{
					var inventory = await JetRestService.GetMerchantSkusInventoryAsync( x ).ConfigureAwait( false );
					return Tuple.Create( x, inventory );
				} ).ConfigureAwait( false );

				var res = products.Where( x => x.Item2 != null ).Select( x => Product.From( x.Item2, x.Item1 ) ).ToList();

				JetLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : res.ToJson() ) );

				return res;
			}
			catch( Exception exception )
			{
				var jetException = new JetException( this.CreateMethodCallInfo(), exception );
				JetLogger.LogTraceException( jetException );
				throw jetException;
			}
		}

		public async Task UpdateInventoryAsync( IEnumerable< Inventory > products )
		{
			string methodParameters = string.Format( "{{products:{0}}}", "products.ToJson()" );
			string mark = Guid.NewGuid().ToString();
			try
			{
				JetLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

				var updateResponse = await products.ProcessInBatchAsync( _batchSize, async x => await JetRestService.PutMerchantSkusInventoryAsync( PutMerchantSkusInventoryRequest.From( x ) ).ConfigureAwait( false ) ).ConfigureAwait( false );

				JetLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : PredefinedValues.NotAvailable ) );
			}
			catch( Exception exception )
			{
				var jetException = new JetException( this.CreateMethodCallInfo(), exception );
				JetLogger.LogTraceException( jetException );
				throw jetException;
			}
		}
		#endregion

		#region Orders
		public async Task< IEnumerable< Order > > GetOrdersAsync( DateTime createDateFrom, DateTime createdDateTo )
		{
			try
			{
				var methodParameters = string.Format( "{{\"createDateFrom\":{0}, \"createdDateTo\":{1}}}", createDateFrom, createdDateTo );
				var mark = Guid.NewGuid().ToString();

				JetLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

				var orderUrls = await JetRestService.GetOrderUrlsAsync().ConfigureAwait( false );
				var orders = await orderUrls.OrderUrls.ProcessInBatchAsync( _batchSize, async s => await JetRestService.GetOrderWithoutShipmentDetailAsync( s ).ConfigureAwait( false ) ).ConfigureAwait( false );
				var ordersFiltered = orders.Where( x => x.OrderPlacedDate < createdDateTo && x.OrderPlacedDate > createDateFrom ).Select( Order.From ).ToList();

				JetLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : ordersFiltered.ToJson() ) );

				return ordersFiltered;
			}
			catch( Exception exception )
			{
				var jetException = new JetException( this.CreateMethodCallInfo(), exception );
				JetLogger.LogTraceException( jetException );
				throw jetException;
			}
		}

		public async Task< IEnumerable< Order > > GetOrdersAsync( params string[] orderIds )
		{
			try
			{
				var methodParameters = string.Format( "{{\"orderIds\":{0}}}", orderIds.ToJson() );
				var mark = Guid.NewGuid().ToString();

				JetLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

				var orderUrls = await JetRestService.GetOrderUrlsAsync().ConfigureAwait( false );
				var orders = await orderUrls.OrderUrls.ProcessInBatchAsync( _batchSize, async s => await JetRestService.GetOrderWithoutShipmentDetailAsync( s ).ConfigureAwait( false ) ).ConfigureAwait( false );
				var ordersFiltered = orders.Where( x => orderIds.Contains( x.MerchantOrderId ) ).Select( Order.From ).ToList();

				JetLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : ordersFiltered.ToJson() ) );

				return ordersFiltered;
			}
			catch( Exception exception )
			{
				var jetException = new JetException( this.CreateMethodCallInfo(), exception );
				JetLogger.LogTraceException( jetException );
				throw jetException;
			}
		}

		public async Task< IEnumerable< Order > > GetOrdersAsync()
		{
			try
			{
				var methodParameters = PredefinedValues.NotAvailable;
				var mark = Guid.NewGuid().ToString();

				JetLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

				var orderUrls = await JetRestService.GetOrderUrlsAsync().ConfigureAwait( false );

				var orderUrlsWithoutShippingDetails = orderUrls.OrderUrls.Where( x => x.Contains( "witho" ) ).ToList();
				//var orderUrlsWithShippingDetails = orderUrls.OrderUrls = orderUrls.OrderUrls.Where( x => x.Contains( "withS" ) ).ToList();

				var ordersWithoutShippingDetails = await orderUrlsWithoutShippingDetails.ProcessInBatchAsync( _batchSize, async s => await JetRestService.GetOrderWithoutShipmentDetailAsync( s ).ConfigureAwait( false ) ).ConfigureAwait( false );
				//var ordersWithShippingDetails = await orderUrlsWithShippingDetails.ProcessInBatchAsync( _batchSize, async s => await JetRestService.GetOrderWithShipmentDetailAsync( s ).ConfigureAwait( false ) ).ConfigureAwait( false );

				var ordersFilteredWithoutShippedDetails = ordersWithoutShippingDetails.Select( Order.From ).ToList();
				//var ordersFilteredWithShippedDetails = ordersWithShippingDetails.Select( Order.From ).ToList();

				var orders = new List< Order >( ordersFilteredWithoutShippedDetails );
				//orders.AddRange( ordersFilteredWithShippedDetails );

				JetLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : orders.ToJson() ) );

				return orders;
			}
			catch( Exception exception )
			{
				var jetException = new JetException( this.CreateMethodCallInfo(), exception );
				JetLogger.LogTraceException( jetException );
				throw jetException;
			}
		}
		#endregion
	}
}