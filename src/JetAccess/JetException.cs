using System;

namespace JetAccess
{
	public class JetException : Exception
	{
		public JetException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}