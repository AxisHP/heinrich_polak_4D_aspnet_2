using System;
using System.Collections.Generic;
using Common.DTO;

namespace heinrich_polak_4D_aspnet_2.Models
{
    public class CartViewModel
    {
        public List<CartItemDTO> Items { get; set; } = new();
        public decimal Total { get; set; }
    }

    public class AddToCartModel
    {
        public Guid ItemPublicId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemModel
    {
        public Guid ItemPublicId { get; set; }
        public int Quantity { get; set; }
    }
}
