using CuttingEdge.Conditions;

namespace JetAccess.Models.Services.JetRestService.GetToken
{
    public class TokenInfo
    {
        public TokenInfo( string token, string tokenType )
        {
            Condition.Requires( token ).IsNotNullOrWhiteSpace();
            Condition.Requires( tokenType ).IsNotNullOrWhiteSpace();

            Token = token;
            TokenType = tokenType;
        }

        public string Token{ get; private set; }
        public string TokenType{ get; private set; }

        public override string ToString()
        {
            return string.Format( "{0} {1}", TokenType, Token );
        }
    }
}