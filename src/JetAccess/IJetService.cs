using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetAccess.Models.GetOrders;
using JetAccess.Models.GetProducts;
using JetAccess.Models.Ping;
using JetAccess.Models.UpdateInventory;

namespace JetAccess
{
    public interface IJetService
    {
        Func< string > AdditionalLogInfo{ get; set; }
        Task< IEnumerable< Order > > GetOrdersAsync( DateTime createDateFrom, DateTime createdDateTo );

        Task< IEnumerable< Order > > GetOrdersAsync();

        Task UpdateInventoryAsync( IEnumerable< Inventory > products );

        Task< IEnumerable< Product > > GetProductsAsync();

        Task< IEnumerable< Order > > GetOrdersAsync( params string[] orderIds );
        Task< PingInfo > Ping();
    }
}