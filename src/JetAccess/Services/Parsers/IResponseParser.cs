using System.IO;
using System.Net;

namespace JetAccess.Services.Parsers
{
    internal interface IResponseParser< T >
    {
        T Parse( Stream stream, bool keepStreamPos );
        T Parse( WebResponse response );
        T Parse( string str );
    }
}