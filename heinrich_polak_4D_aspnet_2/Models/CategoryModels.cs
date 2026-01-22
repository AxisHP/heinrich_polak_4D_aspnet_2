using System;
using System.ComponentModel.DataAnnotations;

namespace heinrich_polak_4D_aspnet_2.Models
{
    public class CreateCategoryModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class UpdateCategoryModel
    {
        public Guid PublicId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class DeleteCategoryModel
    {
        public Guid PublicId { get; set; }
        public string Name { get; set; }
    }
}
