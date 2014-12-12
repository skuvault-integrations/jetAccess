using System.IO;
using JetAccess.Models.Services.JetRestService.GetToken;
using Newtonsoft.Json;

namespace JetAccess.Services.Parsers
{
    internal class GetTokenResponseParser: JsonResponseParser< GetTokenResponse >
    {
        private class ServerResponse
        {
            public string id_token;
            public string token_type;
        }

        public override GetTokenResponse Parse( Stream stream, bool keepStreamPos = true )
        {
            var streamPos = stream.Position;
            var streamReader = new StreamReader( stream );
            var streamStr = streamReader.ReadToEnd();
            var deserializeObject = JsonConvert.DeserializeObject< ServerResponse >( streamStr );

            if( keepStreamPos )
                stream.Seek( streamPos, SeekOrigin.Begin );

            return new GetTokenResponse( deserializeObject.id_token, deserializeObject.token_type );
        }
    }
}