using System.IO;
using JetAccess.Models.Services.JetRestService.GetProductUrls;
using JetAccess.Services.Parsers;
using Newtonsoft.Json;

namespace JetAccess.Services
{
	internal class GetProductUrlsResponseParser: JsonResponseParser< GetProductUrlsResponse >
	{
		private class ServerResponse
		{
			public string[] sku_urls;
		}

		public override GetProductUrlsResponse Parse( Stream stream, bool keepStreamPos = true )
		{
			var streamPos = stream.Position;
			var streamReader = new StreamReader( stream );
			var streamStr = streamReader.ReadToEnd();
			var deserializeObject = JsonConvert.DeserializeObject< ServerResponse >( streamStr );

			if( keepStreamPos )
				stream.Seek( streamPos, SeekOrigin.Begin );

			return new GetProductUrlsResponse( deserializeObject.sku_urls );
		}
	}
}