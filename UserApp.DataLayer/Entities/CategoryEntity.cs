using System;

namespace UserApp.DataLayer.Entities
{
    public class CategoryEntity : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
