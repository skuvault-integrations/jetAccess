using System.IO;
using JetAccess.Models.Services.JetRestService.GetProductsSkus;
using JetAccess.Services.Parsers;
using Newtonsoft.Json;

namespace JetAccess.Services
{
    internal class GetProductsResponseParser: JsonResponseParser< GetProductsResponse >
    {
        private class ServerResponse
        {
            public string[] sku_urls;
        }

        public override GetProductsResponse Parse( Stream stream, bool keepStreamPos = true )
        {
            var streamPos = stream.Position;
            var streamReader = new StreamReader( stream );
            var streamStr = streamReader.ReadToEnd();
            var deserializeObject = JsonConvert.DeserializeObject< ServerResponse >( streamStr );

            if( keepStreamPos )
                stream.Seek( streamPos, SeekOrigin.Begin );

            return new GetProductsResponse( deserializeObject.sku_urls );
        }
    }
}