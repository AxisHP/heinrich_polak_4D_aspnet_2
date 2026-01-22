using System;
using System.Collections.Generic;
using Common.DTO;
using Common.Enums;

namespace heinrich_polak_4D_aspnet_2.Models
{
    public class OrderDetailsModel
    {
        public Guid PublicId { get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
    }

    public class UpdateOrderStatusModel
    {
        public Guid PublicId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
