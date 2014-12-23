using JetAccess.Models;
using JetAccess.Services;

namespace JetAccess
{
	public interface IJetFactory
	{
		IJetService CreateService( JetUserCredentials userAuthCredentials, EndPoint endPoint );
	}
}