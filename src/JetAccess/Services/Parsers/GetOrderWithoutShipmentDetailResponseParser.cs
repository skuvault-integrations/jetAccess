using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetAccess.Models.GetOrderWithOutShipmentDetail;
using Newtonsoft.Json;

namespace JetAccess.Services.Parsers
{
    internal class GetOrderWithoutShipmentDetailResponseParser: JsonResponseParser< GetOrderWithoutShipmentDetailResponse >
    {
        private class ServerResponse
        {
            public string merchant_order_id;
            public string reference_order_id;
            public string fulfillment_node;
            public DateTime order_placed_date;
            public DateTime order_transmission_date;
            public string created;
            public OrderItem[] orderItems;
        }

        private class OrderItem
        {
            public string order_item_id;
            public string merchant_sku;
            public decimal request_order_quantity;
            public ItemPrice itemPrice;
            public string product_title;
            public string url;
        }

        private class ItemPrice
        {
            public decimal base_price;
            public decimal item_tax;
            public decimal item_shipping_cost;
            public decimal item_shipping_tax;
        }

        private static class OrderConverter
        {
            public static GetOrderWithoutShipmentDetailResponse From( ServerResponse deserializeObject )
            {
                var res = new GetOrderWithoutShipmentDetailResponse
                {
                    MerchantOrderId = deserializeObject.merchant_order_id,
                    ReferenceOrderId = deserializeObject.reference_order_id,
                    FulFillmentNode = deserializeObject.fulfillment_node,
                    OrderPlacedDate = deserializeObject.order_placed_date,
                    OrderTransmitionDate = deserializeObject.order_transmission_date,
                    Created = deserializeObject.created,
                    OrderItems = new List< Models.GetOrderWithOutShipmentDetail.OrderItem >(),
                };

                for( var i = 0; i < deserializeObject.orderItems.Count(); i++ )
                {
                    var item = new Models.GetOrderWithOutShipmentDetail.OrderItem
                    {
                        OrderItemId = deserializeObject.orderItems[ i ].order_item_id,
                        MerchantSku = deserializeObject.orderItems[ i ].merchant_sku,
                        RequestOrderQuantity = deserializeObject.orderItems[ i ].request_order_quantity,
                        ProductTitle = deserializeObject.orderItems[ i ].product_title,
                        Url = deserializeObject.orderItems[ i ].url,
                        BasePrice = deserializeObject.orderItems[ i ].itemPrice.base_price,
                        ItemShippingCost = deserializeObject.orderItems[ i ].itemPrice.item_shipping_cost,
                        ItemShippingTax = deserializeObject.orderItems[ i ].itemPrice.item_shipping_tax,
                        ItemTax = deserializeObject.orderItems[ i ].itemPrice.item_tax,
                    };

                    ( ( List< Models.GetOrderWithOutShipmentDetail.OrderItem > )res.OrderItems ).Add( item );
                }

                return res;
            }
        }

        public override GetOrderWithoutShipmentDetailResponse Parse( Stream stream, bool keepStreamPos = true )
        {
            var streamPos = stream.Position;
            var streamReader = new StreamReader( stream );
            var streamStr = streamReader.ReadToEnd();
            var deserializeObject = JsonConvert.DeserializeObject< ServerResponse >( streamStr );

            if( keepStreamPos )
                stream.Seek( streamPos, SeekOrigin.Begin );

            return OrderConverter.From( deserializeObject );
        }
    }
}