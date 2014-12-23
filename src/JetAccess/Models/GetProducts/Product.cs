using System.Collections.Generic;
using System.Linq;
using JetAccess.Misc;
using JetAccess.Models.Services.JetRestService.GetMerchantSkusInventory;

namespace JetAccess.Models.GetProducts
{
	public class Product: IJsonSerializable
	{
		internal static Product From( GetMerchantSkusInventoryResponse getMerchantSkusInventoryResponse, string skuUrl = "" )
		{
			var product = new Product()
			{
				Nodes = getMerchantSkusInventoryResponse.GulfillmentNodes.Select( FullFillmentNode.From ).ToList(),
				SkuUrl = skuUrl,
			};

			return product;
		}

		public string SkuUrl{ get; set; }

		public IEnumerable< FullFillmentNode > Nodes{ get; set; }

		public string ToJson()
		{
			return string.Format( "{{\"SkuUrl\":{0}, \"Nodes\":{1}}}", SkuUrl, Nodes.ToJson() );
		}
	}

	public class FullFillmentNode: IJsonSerializable
	{
		internal static FullFillmentNode From( FulfillmentNode fulfillmentNode )
		{
			var node = new FullFillmentNode
			{
				FulfillmentNodeId = fulfillmentNode.FulfillmentNodeId,
				Quantity = fulfillmentNode.Quantity,
			};
			return node;
		}

		public string FulfillmentNodeId{ get; set; }

		public decimal Quantity{ get; set; }

		public string ToJson()
		{
			return string.Format( "{{\"FulfillmentNodeId\":\"{0}\", \"Quantity\":{1}}}", FulfillmentNodeId, Quantity );
		}
	}
}