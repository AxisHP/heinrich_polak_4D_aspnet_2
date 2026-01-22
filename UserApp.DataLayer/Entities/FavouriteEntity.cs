using System;

namespace UserApp.DataLayer.Entities
{
    public class FavouriteEntity : BaseEntity
    {
        public Guid UserPublicId { get; set; }
        public UserEntity User { get; set; }
        public Guid ItemPublicId { get; set; }
        public ItemEntity Item { get; set; }
    }
}
