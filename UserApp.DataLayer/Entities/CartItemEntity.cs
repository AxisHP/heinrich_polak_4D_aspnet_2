using System;

namespace UserApp.DataLayer.Entities
{
    public class CartItemEntity : BaseEntity
    {
        public Guid UserPublicId { get; set; }
        public UserEntity User { get; set; }
        public Guid ItemPublicId { get; set; }
        public ItemEntity Item { get; set; }
        public int Quantity { get; set; }
    }
}
