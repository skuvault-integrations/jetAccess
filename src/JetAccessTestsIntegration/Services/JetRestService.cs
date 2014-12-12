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
            var service = new JetRestService( _testDataReader.GetJetUserCredentials );

            //------------ Act
            var task = service.GetTokenAsync();
            task.Wait();

            //------------ Assert
            task.Result.Token.Should().NotBeNullOrWhiteSpace();
        }
    }
}