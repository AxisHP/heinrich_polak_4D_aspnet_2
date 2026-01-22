using System;
using System.Collections.Generic;

namespace UserApp.DataLayer.Entities
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ItemEntity> Items { get; set; } = new();
    }
}
