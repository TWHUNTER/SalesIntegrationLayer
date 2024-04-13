using System.ComponentModel.DataAnnotations;

namespace SalesIntegrationLayer.Dtos
{
    public class CreateClientDTO
    {
        [Required]
        public string client_fullname { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string phone_number { get; set; }
    }

    public class UpdateClientDTO
    {
        [Required]
        public string client_fullname { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string phone_number { get; set; }

    }

}