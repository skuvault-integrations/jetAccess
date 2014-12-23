using System.Collections.Generic;
using System.Linq;

namespace JetAccess.Misc
{
	public interface IJsonSerializable
	{
		string ToJson();
	}

	public static class IJsonSerializableExt
	{
		public static string ToJson< T >( this IEnumerable< T > source ) where T : IJsonSerializable
		{
			if( source == null )
				source = new List< T >();

			var list = source.Select( x => x.ToJson() ).ToList();
			var joinedString = string.Join( ",", list );
			var res = string.Format( "[{0}]", joinedString );
			return res;
		}
	}
}