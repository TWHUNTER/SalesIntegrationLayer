
namespace IntegracionDesarrollo3.Models
{
    public class Profile 
    {
        public int ID  { get; set; }
        public string profile_role { get; set; }
        public string role_description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set;}
    }
}
