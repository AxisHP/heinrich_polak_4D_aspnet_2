using System;

namespace Common.DTO
{
    public class CartItemDTO
    {
        public Guid PublicId { get; set; }
        public Guid UserPublicId { get; set; }
        public Guid ItemPublicId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int Quantity { get; set; }
    }
}
