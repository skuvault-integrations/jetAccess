namespace JetAccess.Models.Services.JetRestService.GetToken
{
	public class GetTokenResponse
	{
		public GetTokenResponse( string token, string tokenType )
		{
			TokenInfo = new TokenInfo( token, tokenType );
		}

		public TokenInfo TokenInfo{ get; private set; }
	}
}