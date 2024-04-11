
namespace IntegracionDesarrollo3.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string client_FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime  CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
