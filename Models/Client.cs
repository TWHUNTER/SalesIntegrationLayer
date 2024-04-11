namespace IntegracionDesarrollo3.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string client_fullname { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public DateTime  createdAt { get; set; }

        public DateTime updatedAt { get; set; }
    }
}
