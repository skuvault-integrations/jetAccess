using System.Collections.Generic;

namespace JetAccess.Models.UpdateInventory
{
    public class Inventory
    {
        public Inventory( string id, IEnumerable< FulfillmentNode > nodes3 )
        {
            Id = id;
            Nodes = nodes3;
        }

        public string Id{ get; private set; }
        public IEnumerable< FulfillmentNode > Nodes{ get; private set; }
    }

    public class FulfillmentNode
    {
        public FulfillmentNode( decimal quantity, string fulfillmentNodeId )
        {
            FulfillmentNodeId = fulfillmentNodeId;
            Quantity = quantity;
        }

        public string FulfillmentNodeId{ get; private set; }
        public decimal Quantity{ get; private set; }
    }
}