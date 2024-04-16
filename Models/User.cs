
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegracionDesarrollo3.Models
{
    [Table("users")]
    public class UserModel
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string user_password { get; set; }
        [Column("full_name")]
        public string client_FullName { get; set; }
        public string Email { get; set; }

        // its called phone_number in the database
        [Column("phone_number")]
        public string PhoneNumber { get; set; }
        [Column("profile_type")]
        public int ProfileType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
