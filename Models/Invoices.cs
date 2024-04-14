using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegracionDesarrollo3.Models
{
    public class Invoices
    {
        public int ID { get; set; }

        public int client_id { get; set; }
        public DateTime invoice_date { get; set; }
        public decimal total_amount { get; set; }
        public int payment_method_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
