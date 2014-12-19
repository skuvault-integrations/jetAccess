using System.Collections.Generic;
using JetAccess.Models.Services.JetRestService.PutMerchantSkusInventory;

namespace JetAccess.Models.UpdateInventory
{
    public class Inventory
    {
        public string Id{ get; set; }
        public IEnumerable< FulfillmentNode2 > Nodes{ get; set; }
    }

    public class FulfillmentNode2
    {
        public string FulfillmentNodeId{ get; set; }
        public decimal Quantity{ get; set; }
    }
}