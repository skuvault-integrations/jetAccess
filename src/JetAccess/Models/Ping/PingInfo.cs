namespace JetAccess.Models.Ping
{
	public class PingInfo
	{
		public PingInfo( bool credentialsReceived )
		{
			CredentialsReceived = credentialsReceived;
		}

		public bool CredentialsReceived{ get; private set; }
	}
}