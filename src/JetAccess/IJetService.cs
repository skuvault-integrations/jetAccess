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
        Task< IEnumerable< Order > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo );

        Task< IEnumerable< Order > > GetOrdersAsync();

        Task UpdateInventoryAsync( IEnumerable< Inventory > products );

        Task< IEnumerable< Product > > GetProductsAsync();

        Task< PingInfo > Ping();

        Task< IEnumerable< Order > > GetOrdersAsync( params string[] docNumbers );
    }
}