using System.Threading.Tasks;
using JetAccess.Models;
using JetAccess.Models.Services.JetRestService.GetToken;

namespace JetAccess.Services
{
    public class JetRestService
    {
        public JetRestService( JetUserCredentials getJetUserCredentials )
        {
        }

        public async Task< GetTokenResponse > GetTokenAsync()
        {
            return default( GetTokenResponse );
        }
    }
}