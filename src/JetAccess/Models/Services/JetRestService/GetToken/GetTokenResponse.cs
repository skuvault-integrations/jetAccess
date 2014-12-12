namespace JetAccess.Models.Services.JetRestService.GetToken
{
    public class GetTokenResponse
    {
        public GetTokenResponse( string token, string tokenType )
        {
            Token = token;
            TokenType = tokenType;
        }

        public string Token{ get; private set; }
        public string TokenType{ get; private set; }
    }
}