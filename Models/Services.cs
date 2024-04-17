namespace IntegracionDesarrollo3.Models
{
    public class ServicesModel
    {

        public int Id { get; set; }
        public string service_name { get; set; }
        public string services_description { get; set; }
        public decimal price { get; set; }
        public bool? isVisible { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
