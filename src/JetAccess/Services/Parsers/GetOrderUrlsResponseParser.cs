using System.IO;
using JetAccess.Models.Services.JetRestService.GetOrderIds;
using Newtonsoft.Json;

namespace JetAccess.Services.Parsers
{
    internal class GetOrderUrlsResponseParser: JsonResponseParser< GetOrderUrlsResponse >
    {
        private class ServerResponse
        {
            public string[] order_urls;
        }

        public override GetOrderUrlsResponse Parse( Stream stream, bool keepStreamPos = true )
        {
            var streamPos = stream.Position;
            var streamReader = new StreamReader( stream );
            var streamStr = streamReader.ReadToEnd();
            var deserializeObject = JsonConvert.DeserializeObject< ServerResponse >( streamStr );

            if( keepStreamPos )
                stream.Seek( streamPos, SeekOrigin.Begin );

            return new GetOrderUrlsResponse( deserializeObject.order_urls );
        }
    }
}