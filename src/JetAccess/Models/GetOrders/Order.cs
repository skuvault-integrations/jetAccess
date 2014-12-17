using System;
using System.Collections.Generic;
using System.Linq;
using JetAccess.Models.Services.JetRestService.GetOrderWithOutShipmentDetail;

namespace JetAccess.Models.GetOrders
{
    public class Order
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
                //OrderItems = getOrderWithoutShipmentDetailResponse.OrderItems.Select(OrderLineItem.From).ToList(),
                OrderItems = getOrderWithoutShipmentDetailResponse.OrderItems.Convert< OrderItem, OrderLineItem >(),
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
    }

    public class OrderLineItem : IConvertableFrom<OrderItem,OrderLineItem>
    {
        //internal static OrderLineItem From( OrderItem getOrderWithoutShipmentDetailResponse )
        //{
        //    var orderLineItem = new OrderLineItem
        //    {
        //        BasePrice = getOrderWithoutShipmentDetailResponse.BasePrice,
        //        ItemShippingCost = getOrderWithoutShipmentDetailResponse.ItemShippingCost,
        //        ItemShippingTax = getOrderWithoutShipmentDetailResponse.ItemShippingTax,
        //        ItemTax = getOrderWithoutShipmentDetailResponse.ItemTax,
        //        MerchantSku = getOrderWithoutShipmentDetailResponse.MerchantSku,
        //        OrderItemId = getOrderWithoutShipmentDetailResponse.OrderItemId,
        //        ProductTitle = getOrderWithoutShipmentDetailResponse.ProductTitle,
        //        RequestOrderQuantity = getOrderWithoutShipmentDetailResponse.RequestOrderQuantity,
        //        Url = getOrderWithoutShipmentDetailResponse.Url
        //    };

        //    return orderLineItem;
        //}

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

        //public OrderLineItem From< OrderItem, OrderLineItem >( OrderItem source )
        //{
        //    var orderLineItem = new OrderLineItem
        //    {
        //        BasePrice = getOrderWithoutShipmentDetailResponse.BasePrice,
        //        ItemShippingCost = getOrderWithoutShipmentDetailResponse.ItemShippingCost,
        //        ItemShippingTax = getOrderWithoutShipmentDetailResponse.ItemShippingTax,
        //        ItemTax = getOrderWithoutShipmentDetailResponse.ItemTax,
        //        MerchantSku = getOrderWithoutShipmentDetailResponse.MerchantSku,
        //        OrderItemId = getOrderWithoutShipmentDetailResponse.OrderItemId,
        //        ProductTitle = getOrderWithoutShipmentDetailResponse.ProductTitle,
        //        RequestOrderQuantity = getOrderWithoutShipmentDetailResponse.RequestOrderQuantity,
        //        Url = getOrderWithoutShipmentDetailResponse.Url
        //    };

        //    return orderLineItem;
        //    return ( OrderLineItem )new Object();
        //}
        public OrderLineItem From( OrderItem source )
        {
            return  ;
        }
    }

    public interface IConvertableFrom<TS, TD> where TD : new()
    {
        TD From(TS source);
    }

    public static class IConvertableFromExtensions
    {
        public static IEnumerable< TD > Convert< TS, TD >( this IEnumerable< TS > sourceEnum )
            where TD : IConvertableFrom< TS, TD >, new()
        {
            return sourceEnum.Select( x => new TD().From< TS, TD >( x ) );
        }

        //public static TD From<TS, TD>(TS source) where TD : IConvertableFrom<TS, TD>, new()
        //{
        //    return default( TD );
        //}
    }
}