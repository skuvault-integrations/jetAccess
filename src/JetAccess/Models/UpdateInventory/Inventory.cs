using System;
using System.Collections.Generic;
using System.Configuration;
using JetAccess.Misc;

namespace JetAccess.Models.UpdateInventory
{
	public class Inventory: IJsonSerializable
	{
		public Inventory( string skuUrl, IEnumerable< FulfillmentNode > nodes3 )
		{
			this.SkuUrl = skuUrl;
			this.Nodes = nodes3;
		}

		public string SkuUrl{ get; private set; }
		public IEnumerable< FulfillmentNode > Nodes{ get; private set; }

		public string ToJson()
		{
			return string.Format( "{{\"SkuUrl\":{0}, \"Nodes\":{1}}}", this.SkuUrl, this.Nodes.ToJson() );
		}
	}

	public class FulfillmentNode: IJsonSerializable
	{
		public FulfillmentNode( decimal quantity, string fulfillmentNodeId )
		{
			this.FulfillmentNodeId = fulfillmentNodeId;
			this.Quantity = quantity;
		}

		public string FulfillmentNodeId{ get; private set; }
		public decimal Quantity{ get; private set; }

		public string ToJson()
		{
			return string.Format( "{{\"FulfillmentNodeId\":{0}, \"Quantity\":{1}}}", this.FulfillmentNodeId, this.Quantity );
		}
	}
}