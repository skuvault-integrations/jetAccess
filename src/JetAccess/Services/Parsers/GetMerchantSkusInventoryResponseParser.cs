using System.Collections.Generic;
using System.IO;
using JetAccess.Models.Services.JetRestService.GetMerchantSkusInventory;
using Newtonsoft.Json;

namespace JetAccess.Services.Parsers
{
    internal class GetMerchantSkusInventoryResponseParser: JsonResponseParser< GetMerchantSkusInventoryResponse >
    {
        private class fulfillment_node
        {
            public string fulfillment_node_id;
            public decimal quantity;
        }

        private class ServerResponse
        {
            public fulfillment_node[] fulfillment_nodes;
        }

        public override GetMerchantSkusInventoryResponse Parse( Stream stream, bool keepStreamPos = true )
        {
            var streamPos = stream.Position;
            var streamReader = new StreamReader( stream );
            var streamStr = streamReader.ReadToEnd();
            var deserializeObject = JsonConvert.DeserializeObject< ServerResponse >( streamStr );

            if( keepStreamPos )
                stream.Seek( streamPos, SeekOrigin.Begin );

            var nodes = new List< FulfillmentNode >();
            foreach( var node in deserializeObject.fulfillment_nodes )
            {
                var fulfillmentNode = new FulfillmentNode( node.fulfillment_node_id, node.quantity );
                nodes.Add( fulfillmentNode );
            }

            return new GetMerchantSkusInventoryResponse( nodes );
        }
    }
}