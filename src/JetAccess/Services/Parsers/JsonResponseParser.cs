using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace JetAccess.Services.Parsers
{
    internal class JsonResponseParser< TParseResult >: IResponseParser< TParseResult >
    {
        //protected ResponseError ResponseContainsErrors(XElement root, XNamespace ns)
        //{
        //    var messages = root.Element(ns + "messages");

        //    if (messages == null)
        //        return null;

        //    var errorCode = GetElementValue(messages, ns, "error", "data_item", "code");
        //    var errorMessage = GetElementValue(messages, ns, "error", "data_item", "message");

        //    var ResponseError = new ResponseError { Code = errorCode, Message = errorMessage };
        //    return ResponseError;
        //}

        public TParseResult Parse( WebResponse response )
        {
            var result = default( TParseResult );
            using( var responseStream = response.GetResponseStream() )
            {
                if( responseStream != null )
                {
                    using( var memStream = new MemoryStream() )
                    {
                        responseStream.CopyTo( memStream, 0x100 );
                        result = this.Parse( memStream );
                    }
                }
            }

            return result;
        }

        public TParseResult Parse( string str )
        {
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter( stream );
            streamWriter.Write( str );
            streamWriter.Flush();
            stream.Position = 0;

            using( stream )
                return Parse( stream );
        }

        public virtual TParseResult Parse( Stream stream, bool keepStreamPos = true )
        {
            var streamStartPos = stream.Position;

            try
            {
                var root = XElement.Load( stream );
                return ( TParseResult )new Object();
            }
            catch( Exception ex )
            {
                var buffer = new byte[ stream.Length ];
                stream.Read( buffer, 0, ( int )stream.Length );
                var utf8Encoding = new UTF8Encoding();
                var bufferStr = utf8Encoding.GetString( buffer );
                throw new Exception( "Can't parse: " + bufferStr, ex );
            }
            finally
            {
                if( keepStreamPos )
                    stream.Position = streamStartPos;
            }
        }
    }
}