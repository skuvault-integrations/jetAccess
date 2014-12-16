using System.Linq;
using FluentAssertions;
using JetAccess.Services;
using JetAccessTestsIntegration.TestEnvironment;
using NUnit.Framework;

namespace JetAccessTestsIntegration.Services
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
    }
}