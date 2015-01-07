using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetAccess;
using JetAccess.Models.GetProducts;
using JetAccess.Models.UpdateInventory;
using JetAccess.Services;
using JetAccessTestsIntegration.TestEnvironment;
using NUnit.Framework;

namespace JetAccessTestsIntegration
{
	public class JetServiceTest
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
			var service = new JetService( _testDataReader.GetJetUserCredentials, EndPoint.Test );

			//------------ Act
			var task = service.PingAsync();
			task.Wait();

			//------------ Assert
			task.Result.CredentialsReceived.Should().BeTrue();
		}

		[ Test ]
		public async Task GetOrders_PasswordsAndConnectionAreGood_OrderIdsReceived()
		{
			//------------ Arrange
			var service = new JetService( _testDataReader.GetJetUserCredentials, EndPoint.Test );

			//------------ Act
			var task = await service.GetOrdersAsync().ConfigureAwait( false );
			//task.Wait();

			//------------ Assert
			//task.Result.Should().HaveCount( x => x > 0 );
			task.Should().HaveCount( x => x > 0 );
		}

		[ Test ]
		public void GetProducts_PasswordsAndConnectionAreGood_ProductsUrlsReceived()
		{
			//------------ Arrange
			var service = new JetService( _testDataReader.GetJetUserCredentials, EndPoint.Test );

			//------------ Act
			var task = service.GetProductsAsync();
			task.Wait();

			//------------ Assert
			task.Result.Should().HaveCount( x => x > 0 );
		}

		[ Test ]
		public void UpdateInventoryAsync_PasswordsAndConnectionAreGood_ProductsInventoryUpdated()
		{
			//------------ Arrange
			var service = new JetService( _testDataReader.GetJetUserCredentials, EndPoint.Test );
			var task2 = service.GetProductsAsync();
			task2.Wait();

			//------------ Act
			var updateProducts = new List< Inventory >();
			foreach( var source in task2.Result.ToList() )
			{
				var id = source.SkuUrl;
				var nodes3 = source.Nodes.Select( x => new FulfillmentNode( x.Quantity + 1, x.FulfillmentNodeId ) ).ToList();
				var inventory = new Inventory( id, nodes3 );
				updateProducts.Add( inventory );
			}

			var task3 = service.UpdateInventoryAsync( updateProducts );
			task3.Wait();

			var task4 = service.GetProductsAsync();
			task4.Wait();

			//------------ Assert
			task4.Result.Should().OnlyContain( x => IsCorrespondNodesDiffersBy( x, task2.Result.First( y => y.SkuUrl == x.SkuUrl ) ) );
		}

		private static bool IsCorrespondNodesDiffersBy( Product x, Product productBeforeUpdate )
		{
			foreach( var nodeAfterUpdate in x.Nodes )
			{
				var nodeBeforeUpdate = productBeforeUpdate.Nodes.First( z => z.FulfillmentNodeId == nodeAfterUpdate.FulfillmentNodeId );
				if( ( nodeAfterUpdate.Quantity - nodeBeforeUpdate.Quantity ) != 1 )
					return false;
			}
			return true;
		}
	}
}