using System;
using System.Collections.Generic;

namespace UserApp.DataLayer.Entities
{
    public class OrderEntity : BaseEntity
    {
        public Guid UserPublicId { get; set; }
        public UserEntity User { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public Common.Enums.OrderStatus Status { get; set; }
    }
}
