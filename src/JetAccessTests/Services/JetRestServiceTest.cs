using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetAccess;
using JetAccess.Models.GetProducts;
using JetAccess.Models.Services.JetRestService.PutMerchantSkusInventory;
using JetAccess.Models.UpdateInventory;
using JetAccess.Services;
using JetAccessTests.TestEnvironment;
using NUnit.Framework;
using FulfillmentNode = JetAccess.Models.Services.JetRestService.PutMerchantSkusInventory.FulfillmentNode;

namespace JetAccessTests.Services
{
	[ TestFixture ]
	public class JetRestServiceTest
	{
		private TestDataReader _testDataReader;

		[ TestFixtureSetUp ]
		public void Setup()
		{
			this._testDataReader = new TestDataReader( @"..\..\Files\UserCredentials.csv" );
		}

		[ Test ]
		public void GetToken_PasswordsAndConnectionAreGood_TokenReceived()
		{
			//------------ Arrange
			var service = new JetRestService( _testDataReader.GetJetUserCredentials, EndPoint.Test );

			//------------ Act
			var task = service.GetTokenAsync();
			task.Wait();

			//------------ Assert
			task.Result.TokenInfo.Should().NotBeNull();
		}

		[ Test ]
		public void GetOrders_PasswordsAndConnectionAreGood_OrderIdsReceived()
		{
			//------------ Arrange
			var service = new JetRestService( _testDataReader.GetJetUserCredentials, EndPoint.Test );

			//------------ Act
			var task = service.GetOrderUrlsAsync();
			task.Wait();

			//------------ Assert
			task.Result.OrderUrls.Should().HaveCount( x => x > 0 );
		}

		[ Test ]
		public void GetOrdersWithOutShippedDetails_PasswordsAndConnectionAreGood_OrderIdsReceived()
		{
			//------------ Arrange
			var service = new JetRestService( _testDataReader.GetJetUserCredentials, EndPoint.Test );

			//------------ Act
			var task = service.GetOrderUrlsAsync();
			task.Wait();
			var task2 = service.GetOrderWithoutShipmentDetailAsync( task.Result.OrderUrls.First() );
			task2.Wait();

			//------------ Assert
			task2.Result.MerchantOrderId.Should().NotBeNullOrWhiteSpace();
		}

		[ Test ]
		public void GetProductUrls_PasswordsAndConnectionAreGood_ProductsUrlsReceived()
		{
			//------------ Arrange
			var service = new JetRestService( _testDataReader.GetJetUserCredentials, EndPoint.Test );

			//------------ Act
			var task = service.GetProductUrlsAsync();
			task.Wait();

			//------------ Assert
			task.Result.SkuUrls.Should().HaveCount( x => x > 0 );
		}

		[Test]
		public void GetProducts()
		{
			//------------ Arrange
			var service = new JetService(_testDataReader.GetJetUserCredentials, EndPoint.Test);

			//------------ Act
			var task = service.GetProductsAsync();
			task.Wait();

			//------------ Assert
			task.Result.Should().HaveCount(x => x > 0);
		}

		[ Test ]
		public void GetMerchantSkusInventory_PasswordsAndConnectionAreGood_ProductsInventoryReceived()
		{
			//------------ Arrange
			var service = new JetRestService( _testDataReader.GetJetUserCredentials, EndPoint.Test );

			//------------ Act
			var task = service.GetProductUrlsAsync();
			task.Wait();
			var task2 = service.GetMerchantSkusInventoryAsync( task.Result.SkuUrls.First() );
			task2.Wait();

			//------------ Assert
			task2.Result.GulfillmentNodes.Should().OnlyContain( x => !string.IsNullOrWhiteSpace( x.FulfillmentNodeId ) );
		}

		[Test]
		public async Task UpdateQty()
		{
			var service = new JetService(_testDataReader.GetJetUserCredentials, EndPoint.Test);
			
			var products = new List<Inventory>();
			var nodes = new List<JetAccess.Models.UpdateInventory.FulfillmentNode>();
			nodes.Add(new JetAccess.Models.UpdateInventory.FulfillmentNode(3m, "5553d2a6d45a467e9fffb29f7949df20"));

			products.Add(new Inventory("testSku1", nodes));

			await service.UpdateInventoryAsync(products);
		}

		[ Test ]
		public void PutMerchantSkusInventory_PasswordsAndConnectionAreGood_ProductsInventoryUpdated()
		{
			//------------ Arrange
			var service = new JetRestService( _testDataReader.GetJetUserCredentials, EndPoint.Test );
			var task1 = service.GetProductUrlsAsync();
			task1.Wait();
			var task2 = service.GetMerchantSkusInventoryAsync( task1.Result.SkuUrls.First() );
			task2.Wait();

			//------------ Act
			var quantity = task2.Result.GulfillmentNodes.First().Quantity;
			IEnumerable< FulfillmentNode > nodes = new List< FulfillmentNode > { new FulfillmentNode( task2.Result.GulfillmentNodes.First().FulfillmentNodeId, quantity + 1 ) };
			PutMerchantSkusInventoryRequest putMerchantSkusInventoryRequest = new PutMerchantSkusInventoryRequest( task1.Result.SkuUrls.First(), nodes );
			var task3 = service.PutMerchantSkusInventoryAsync( putMerchantSkusInventoryRequest );
			task3.Wait();

			var task4 = service.GetMerchantSkusInventoryAsync( task1.Result.SkuUrls.First() );
			task4.Wait();

			//------------ Assert
			task4.Result.GulfillmentNodes.Should().OnlyContain( x => x.Quantity == ( quantity + 1 ) );
		}
	}
}