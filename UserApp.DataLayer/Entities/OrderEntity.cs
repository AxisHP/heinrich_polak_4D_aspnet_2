using System;
using System.Collections.Generic;

namespace UserApp.DataLayer.Entities
{
    public class OrderEntity : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public Common.Enums.OrderStatus Status { get; set; }
    }
}
