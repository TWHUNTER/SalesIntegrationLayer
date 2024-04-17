using System.ComponentModel.DataAnnotations;

namespace SalesIntegrationLayer.Dtos
{
    public class CreateProfileDTO
    {
        [Required]
        public string profile_role { get; set; }
        public string role_description { get; set; }


    }
}