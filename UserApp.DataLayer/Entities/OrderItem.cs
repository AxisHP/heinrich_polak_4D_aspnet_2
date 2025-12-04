using System;

namespace UserApp.DataLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
