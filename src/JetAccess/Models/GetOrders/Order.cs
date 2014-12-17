using System;
using System.Collections.Generic;
using System.Linq;
using JetAccess.Misc;
using JetAccess.Models.Services.JetRestService.GetOrderWithOutShipmentDetail;

namespace JetAccess.Models.GetOrders
{
    public class Order: IJsonSerializable
    {
        internal static Order From( GetOrderWithoutShipmentDetailResponse getOrderWithoutShipmentDetailResponse )
        {
            var order = new Order
            {
                Created = getOrderWithoutShipmentDetailResponse.Created,
                FulFillmentNode = getOrderWithoutShipmentDetailResponse.FulFillmentNode,
                MerchantOrderId = getOrderWithoutShipmentDetailResponse.MerchantOrderId,
                OrderPlacedDate = getOrderWithoutShipmentDetailResponse.OrderPlacedDate,
                OrderTransmitionDate = getOrderWithoutShipmentDetailResponse.OrderTransmitionDate,
                ReferenceOrderId = getOrderWithoutShipmentDetailResponse.ReferenceOrderId,
                OrderItems = getOrderWithoutShipmentDetailResponse.OrderItems.Select( OrderLineItem.From ).ToList(),
            };

            return order;
        }

        public IEnumerable< OrderLineItem > OrderItems{ get; set; }

        public string ReferenceOrderId{ get; set; }

        public DateTime OrderTransmitionDate{ get; set; }

        public DateTime OrderPlacedDate{ get; set; }

        public string MerchantOrderId{ get; set; }

        public string FulFillmentNode{ get; set; }

        public string Created{ get; set; }

        public string ToJson()
        {
            return string.Format( "{{MerchantOrderId:\"{0}\", Created:\"{1}\", RequestOrderQuantity:\"{2}\"}}", this.MerchantOrderId, this.Created, this.OrderItems.ToJson() );
        }
    }

    public class OrderLineItem: IJsonSerializable
    {
        internal static OrderLineItem From( OrderItem getOrderWithoutShipmentDetailResponse )
        {
            var orderLineItem = new OrderLineItem
            {
                BasePrice = getOrderWithoutShipmentDetailResponse.BasePrice,
                ItemShippingCost = getOrderWithoutShipmentDetailResponse.ItemShippingCost,
                ItemShippingTax = getOrderWithoutShipmentDetailResponse.ItemShippingTax,
                ItemTax = getOrderWithoutShipmentDetailResponse.ItemTax,
                MerchantSku = getOrderWithoutShipmentDetailResponse.MerchantSku,
                OrderItemId = getOrderWithoutShipmentDetailResponse.OrderItemId,
                ProductTitle = getOrderWithoutShipmentDetailResponse.ProductTitle,
                RequestOrderQuantity = getOrderWithoutShipmentDetailResponse.RequestOrderQuantity,
                Url = getOrderWithoutShipmentDetailResponse.Url
            };

            return orderLineItem;
        }

        public OrderLineItem()
        {
        }

        public string Url{ get; set; }

        public decimal RequestOrderQuantity{ get; set; }

        public string ProductTitle{ get; set; }

        public string OrderItemId{ get; set; }

        public string MerchantSku{ get; set; }

        public decimal ItemTax{ get; set; }

        public decimal ItemShippingTax{ get; set; }

        public decimal ItemShippingCost{ get; set; }

        public decimal BasePrice{ get; set; }

        public string ToJson()
        {
            return string.Format( "{{MerchantSku:\"{0}\", OrderItemId:\"{1}\", RequestOrderQuantity:\"{2}\"}}", this.MerchantSku, this.OrderItemId, this.RequestOrderQuantity );
        }
    }
}