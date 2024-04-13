using System.ComponentModel.DataAnnotations;

namespace IntegracionDesarrollo3.Dtos
{
    public class ProductsDTO
    {

        [Required]
        public string product_name { get; set; }

        [Required]
        public int stock { get; set; }

        [Required]
        public int category_id { get; set; }

        [Required]
        public decimal price { get; set; }

        [Required]
        public bool isVisible { get; set; }

        [Required]
        [Url]
        public string url_image { get; set; }
    }

    public class UpdateProductDTO
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string product_name { get; set; }

        [Required]
        public int stock { get; set; }

        [Required]
        public int category_id { get; set; }

        [Required]
        public decimal price { get; set; }

        [Required]
        public bool isVisible { get; set; }

        [Required]
        [Url]
        public string url_image { get; set; }
    }

}
