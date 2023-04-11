using System.ComponentModel.DataAnnotations;

namespace ReferenceAPI.DTO
{
    public class EditCategoryDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }
    }
}
