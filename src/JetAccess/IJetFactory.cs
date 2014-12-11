using JetAccess.Models;

namespace JetAccess
{
	public interface IJetFactory
	{
		IJetService CreateService(JetUserCredentials userAuthCredentials);
	}
}