using JetAccess.Models;
using QuickBooksOnlineAccess;

namespace JetAccess
{
	public class JetFactory : IJetFactory
	{
		private readonly JetUserCredentials _nonAuthenticatedQuickBooksOnlineNonAuthenticatedUserCredentials;

		public JetFactory(JetUserCredentials quickBooksOnlineNonAuthenticatedUserCredentials)
		{
			_nonAuthenticatedQuickBooksOnlineNonAuthenticatedUserCredentials = quickBooksOnlineNonAuthenticatedUserCredentials;
		}

		public IJetService CreateService(JetUserCredentials userAuthCredentials)
		{
			return new JetService(userAuthCredentials);
		}
	}
}