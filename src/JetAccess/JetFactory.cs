using JetAccess.Models;
using JetAccess.Services;

namespace JetAccess
{
    public class JetFactory: IJetFactory
    {
        private readonly JetUserCredentials _nonAuthenticatedJetUserCredentials;
        private EndPoint _endPoint;

        public JetFactory( JetUserCredentials jetUserCredentials, EndPoint endPoint )
        {
            _nonAuthenticatedJetUserCredentials = jetUserCredentials;
            _endPoint = endPoint;
        }

        public IJetService CreateService( JetUserCredentials userAuthCredentials, EndPoint endPoint )
        {
            return new JetService( userAuthCredentials, endPoint );
        }
    }
}