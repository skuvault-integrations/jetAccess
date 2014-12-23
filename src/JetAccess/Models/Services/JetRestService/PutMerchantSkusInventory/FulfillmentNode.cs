using System.Collections.Generic;
using System.Linq;
using JetAccess.Models.UpdateInventory;

namespace JetAccess.Models.Services.JetRestService.PutMerchantSkusInventory
{
	internal class FulfillmentNode
	{
		public string FulfillmentNodeId{ get; private set; }
		public decimal Quantity{ get; private set; }

		public FulfillmentNode( string fulfillmentNodeId, decimal quantity )
		{
			FulfillmentNodeId = fulfillmentNodeId;
			Quantity = quantity;
		}

		public string ToJson()
		{
			return string.Format( "{{\"fulfillment_node_id\":\"{0}\",\"quantity\":{1}}}", this.FulfillmentNodeId, this.Quantity );
		}

		internal static FulfillmentNode From( UpdateInventory.FulfillmentNode fulfillmentNode )
		{
			var res = new FulfillmentNode( fulfillmentNode.FulfillmentNodeId, fulfillmentNode.Quantity );
			return res;
		}
	}

	internal class PutMerchantSkusInventoryRequest
	{
		public IEnumerable< FulfillmentNode > FulfillmentNodes{ get; private set; }
		public string Id{ get; private set; }

		public PutMerchantSkusInventoryRequest( string id, IEnumerable< FulfillmentNode > nodes )
		{
			Id = id;
			FulfillmentNodes = nodes;
		}

		public string ToJson()
		{
			var strings = FulfillmentNodes.Select( x => x.ToJson() );
			var list = string.Join( ",", strings );
			var obj = string.Format( "{{\"fulfillment_nodes\":[{0}]}}", list );
			return obj;
		}

		internal static PutMerchantSkusInventoryRequest From( Inventory o )
		{
			var id = o.Id;
			var inventories = o.Nodes.Select( FulfillmentNode.From ).ToList();
			var res = new PutMerchantSkusInventoryRequest( id, inventories );
			return res;
		}
	}
}