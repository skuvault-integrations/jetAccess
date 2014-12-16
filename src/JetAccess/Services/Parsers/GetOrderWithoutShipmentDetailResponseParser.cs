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
            public string status;
            public OrderItem[] order_items;
        }

        private class OrderItem
        {
            public string order_item_id;
            public string merchant_sku;
            public decimal request_order_quantity;
            public item_price item_price;
            public string product_title;
            public string url;
        }

        private class item_price
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
                    Created = deserializeObject.status,
                    OrderItems = new List< Models.GetOrderWithOutShipmentDetail.OrderItem >(),
                };

                for( var i = 0; i < deserializeObject.order_items.Count(); i++ )
                {
                    var item = new Models.GetOrderWithOutShipmentDetail.OrderItem
                    {
                        OrderItemId = deserializeObject.order_items[ i ].order_item_id,
                        MerchantSku = deserializeObject.order_items[ i ].merchant_sku,
                        RequestOrderQuantity = deserializeObject.order_items[ i ].request_order_quantity,
                        ProductTitle = deserializeObject.order_items[ i ].product_title,
                        Url = deserializeObject.order_items[ i ].url,
                        BasePrice = deserializeObject.order_items[ i ].item_price.base_price,
                        ItemShippingCost = deserializeObject.order_items[ i ].item_price.item_shipping_cost,
                        ItemShippingTax = deserializeObject.order_items[ i ].item_price.item_shipping_tax,
                        ItemTax = deserializeObject.order_items[ i ].item_price.item_tax,
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