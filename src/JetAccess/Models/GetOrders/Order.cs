using System;
using System.Collections.Generic;
using System.Linq;
using JetAccess.Misc;
using JetAccess.Models.Services.JetRestService.GetOrderWithOutShipmentDetail;
using JetAccess.Models.Services.JetRestService.GetOrderWithShipmentDetail;
using OrderItem = JetAccess.Models.Services.JetRestService.GetOrderWithOutShipmentDetail.OrderItem;

namespace JetAccess.Models.GetOrders
{
	public class Order: IJsonSerializable
	{
		internal static Order From( GetOrderWithoutShipmentDetailResponse getOrderWithoutShipmentDetailResponse )
		{
			var order = new Order
			{
				Status = getOrderWithoutShipmentDetailResponse.Status,
				ShippingToAddress1 = getOrderWithoutShipmentDetailResponse.ShippingToAddress1,
				ShippingToAddress2 = getOrderWithoutShipmentDetailResponse.ShippingToAddress2,
				ShippingToCity = getOrderWithoutShipmentDetailResponse.ShippingToCity,
				ShippingToSate = getOrderWithoutShipmentDetailResponse.ShippingToSate,
				ShippingToZipCode = getOrderWithoutShipmentDetailResponse.ShippingToZipCode,
				FulFillmentNode = getOrderWithoutShipmentDetailResponse.FulFillmentNode,
				HasShipments = getOrderWithoutShipmentDetailResponse.HasShipments,
				MerchantOrderId = getOrderWithoutShipmentDetailResponse.MerchantOrderId,
				OrderPlacedDate = getOrderWithoutShipmentDetailResponse.OrderPlacedDate,
				OrderTransmitionDate = getOrderWithoutShipmentDetailResponse.OrderTransmitionDate,
				ReferenceOrderId = getOrderWithoutShipmentDetailResponse.ReferenceOrderId,
				OrderItems = getOrderWithoutShipmentDetailResponse.OrderItems.Select( OrderLineItem.From ).ToList(),
			};

			return order;
		}

		internal static Order From( GetOrderWithShipmentDetailResponse getOrderWithoutShipmentDetailResponse )
		{
			var order = new Order
			{
				Status = getOrderWithoutShipmentDetailResponse.Created,
				FulFillmentNode = getOrderWithoutShipmentDetailResponse.FulFillmentNode,
				MerchantOrderId = getOrderWithoutShipmentDetailResponse.MerchantOrderId,
				OrderPlacedDate = getOrderWithoutShipmentDetailResponse.OrderPlacedDate,
				OrderTransmitionDate = getOrderWithoutShipmentDetailResponse.OrderTransmitionDate,
				ReferenceOrderId = getOrderWithoutShipmentDetailResponse.ReferenceOrderId,
				OrderItems = getOrderWithoutShipmentDetailResponse.OrderItems.Select( OrderLineItem.From ).ToList(),
			};

			return order;
		}

		public OrderStatus GetOrderStatus()
		{
			if( this.HasShipments )
				return OrderStatus.Shipped;

			if( !this.HasShipments )
				return OrderStatus.Pending;

			return OrderStatus.Unknown;
		}

		public string ShippingToZipCode{ get; set; }

		public string ShippingToSate{ get; set; }

		public string ShippingToCity{ get; set; }

		public string ShippingToAddress2{ get; set; }

		public string ShippingToAddress1{ get; set; }

		public bool HasShipments{ get; set; }

		public IEnumerable< OrderLineItem > OrderItems{ get; set; }

		public string ReferenceOrderId{ get; set; }

		public DateTime OrderTransmitionDate{ get; set; }

		public DateTime OrderPlacedDate{ get; set; }

		public string MerchantOrderId{ get; set; }

		public string FulFillmentNode{ get; set; }

		public string Status{ get; set; }

		public string ToJson()
		{
			return string.Format( "{{MerchantOrderId:\"{0}\", Status:\"{1}\", RequestOrderQuantity:\"{2}\"}}", this.MerchantOrderId, this.Status, this.OrderItems.ToJson() );
		}
	}

	public enum OrderStatus
	{
		Unknown,
		Shipped,
		Pending
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

		internal static OrderLineItem From( Services.JetRestService.GetOrderWithShipmentDetail.OrderItem getOrderWithoutShipmentDetailResponse )
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