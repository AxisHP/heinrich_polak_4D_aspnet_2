using System;

namespace UserApp.DataLayer.Entities
{
    public class ItemEntity : BaseEntity
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
