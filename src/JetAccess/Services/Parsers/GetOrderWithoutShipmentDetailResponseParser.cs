using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetAccess.Models.Services.JetRestService.GetOrderWithOutShipmentDetail;
using Newtonsoft.Json;

namespace JetAccess.Services.Parsers
{
	internal class GetOrderWithoutShipmentDetailResponseParser: JsonResponseParser< GetOrderWithoutShipmentDetailResponse >
	{
		private class ServerResponse
		{
#pragma warning disable 0649
			public string merchant_order_id;
			public string reference_order_id;
			public string fulfillment_node;
			public bool has_shipments;
			public DateTime order_placed_date;
			public DateTime order_transmission_date;
			public string status;
			public OrderItem[] order_items;
			public ShippingTo shipping_to;
#pragma warning restore 0649
		}

		private class ShippingTo
		{
#pragma warning disable 0649
			public Address address;
			public Recipient recipient;
#pragma warning restore 0649
		}

		private class Address
		{
#pragma warning disable 0649
			public string address1;
			public string address2;
			public string city;
			public string state;
			public string zip_code;
#pragma warning restore 0649
		}

		private class Recipient
		{
#pragma warning disable 0649
			public string name;
			public string phone_number;
#pragma warning restore 0649
		}

		private class OrderItem
		{
#pragma warning disable 0649
			public string order_item_id;
			public string merchant_sku;
			public decimal request_order_quantity;
			public item_price item_price;
			public string product_title;
			public string url;
#pragma warning restore 0649
		}

		private class item_price
		{
#pragma warning disable 0649
			public decimal base_price;
			public decimal? item_tax;
			public decimal? item_shipping_cost;
			public decimal? item_shipping_tax;
#pragma warning restore 0649
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
					HasShipments = deserializeObject.has_shipments,
					OrderPlacedDate = deserializeObject.order_placed_date,
					OrderTransmitionDate = deserializeObject.order_transmission_date,
					Status = deserializeObject.status,
					ShippingToAddress1 = deserializeObject.shipping_to.address.address1,
					ShippingToAddress2 = deserializeObject.shipping_to.address.address1,
					ShippingToCity = deserializeObject.shipping_to.address.city,
					ShippingToSate = deserializeObject.shipping_to.address.state,
					ShippingToZipCode = deserializeObject.shipping_to.address.zip_code,
					OrderItems = new List< Models.Services.JetRestService.GetOrderWithOutShipmentDetail.OrderItem >(),
				};

				for( var i = 0; i < deserializeObject.order_items.Count(); i++ )
				{
					var item = new Models.Services.JetRestService.GetOrderWithOutShipmentDetail.OrderItem
					{
						OrderItemId = deserializeObject.order_items[ i ].order_item_id,
						MerchantSku = deserializeObject.order_items[ i ].merchant_sku,
						RequestOrderQuantity = deserializeObject.order_items[ i ].request_order_quantity,
						ProductTitle = deserializeObject.order_items[ i ].product_title,
						Url = deserializeObject.order_items[ i ].url,
						BasePrice = deserializeObject.order_items[ i ].item_price.base_price,
						ItemShippingCost = deserializeObject.order_items[ i ].item_price.item_shipping_cost ?? 0,
						ItemShippingTax = deserializeObject.order_items[ i ].item_price.item_shipping_tax ?? 0,
						ItemTax = deserializeObject.order_items[ i ].item_price.item_tax ?? 0,
					};

					( ( List< Models.Services.JetRestService.GetOrderWithOutShipmentDetail.OrderItem > )res.OrderItems ).Add( item );
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