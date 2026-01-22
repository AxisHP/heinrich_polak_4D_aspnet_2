using System;

namespace Common.DTO
{
    public class FavoriteDTO
    {
        public Guid PublicId { get; set; }
        public Guid UserPublicId { get; set; }
        public Guid ItemPublicId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
