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
#pragma warning disable 0649
			public string fulfillment_node_id;
			public decimal quantity;
#pragma warning restore 0649
		}

		private class ServerResponse
		{
#pragma warning disable 0649
			public fulfillment_node[] fulfillment_nodes;
#pragma warning restore 0649
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