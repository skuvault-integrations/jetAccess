namespace JetAccess.Models.Services.JetRestService.GetToken
{
    public class GetTokenResponse
    {
        GetTokenResponse(string token)
        {
            Token = token;
        }
        public string Token{ get; private set; }
    }
}