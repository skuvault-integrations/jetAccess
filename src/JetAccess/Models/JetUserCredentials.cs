using CuttingEdge.Conditions;

namespace JetAccess.Models
{
	public class JetUserCredentials
	{
		public string ApiUser{ get; private set; }
		public string Secret{ get; private set; }

		public JetUserCredentials( string apiUser, string secret )
		{
			Condition.Requires( apiUser, "apiUser" ).IsNotNullOrWhiteSpace();
			Condition.Requires( secret, "secret" ).IsNotNullOrWhiteSpace();

			ApiUser = apiUser;
			Secret = secret;
		}
	}
}