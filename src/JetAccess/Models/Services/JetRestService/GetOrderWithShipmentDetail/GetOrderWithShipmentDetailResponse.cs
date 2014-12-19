using System;
using System.Collections.Generic;

namespace JetAccess.Models.Services.JetRestService.GetOrderWithShipmentDetail
{
    internal class GetOrderWithShipmentDetailResponse
    {
        public string MerchantOrderId{ get; set; }
        public string ReferenceOrderId{ get; set; }
        public string FulFillmentNode{ get; set; }
        public DateTime OrderPlacedDate{ get; set; }
        public DateTime OrderTransmitionDate{ get; set; }
        public string Created{ get; set; }
        public IEnumerable< OrderItem > OrderItems{ get; set; }
    }

    internal class OrderItem
    {
        public decimal BasePrice{ get; set; }
        public decimal ItemShippingCost{ get; set; }
        public decimal ItemShippingTax{ get; set; }
        public decimal ItemTax{ get; set; }
        public string OrderItemId{ get; set; }
        public string MerchantSku{ get; set; }
        public decimal RequestOrderQuantity{ get; set; }
        public string ProductTitle{ get; set; }
        public string Url{ get; set; }
    }
}