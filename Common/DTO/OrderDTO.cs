using System;
using System.Collections.Generic;
using Common.Enums;

namespace Common.DTO
{
    public class OrderDTO
    {
        public Guid PublicId { get; set; }
        public Guid UserPublicId { get; set; }
        public string UserName { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class OrderItemDTO
    {
        public Guid PublicId { get; set; }
        public Guid ItemPublicId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
