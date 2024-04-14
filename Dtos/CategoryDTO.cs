using System.ComponentModel.DataAnnotations;

namespace IntegracionDesarrollo3.Dtos
{
    public class CategoryDTO
    {
        [Required]
        public string category_name {  get; set; }
    }

    public class UpdateCategoryDTO
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string category_name { get; set; }
    }
}
