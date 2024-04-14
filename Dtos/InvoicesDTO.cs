namespace IntegracionDesarrollo3.Dtos
{
    namespace IntegracionDesarrollo3.Dtos
    {
        public class CreateInvoiceDTO
        {
            public int ID { get; set; }
            public int client_id { get; set; }
            public DateTime invoice_date { get; set; }
            public int total_amount { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }
    }

}
