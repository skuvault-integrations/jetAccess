using System.Collections.Generic;

namespace JetAccess.Models.Services.JetRestService.GetMerchantSkusInventory
{
    internal class GetMerchantSkusInventoryResponse
    {
        public IEnumerable< FulfillmentNode > GulfillmentNodes{ get; private set; }

        public GetMerchantSkusInventoryResponse( IEnumerable< FulfillmentNode > nodes )
        {
            GulfillmentNodes = nodes;
        }
    }

    internal class FulfillmentNode
    {
        public string FulfillmentNodeId{ get; private set; }
        public decimal Quantity{ get; private set; }

        public FulfillmentNode( string fulfillmentNodeId, decimal quantity )
        {
            Quantity = quantity;
            FulfillmentNodeId = fulfillmentNodeId;
        }
    }
}