namespace IntegracionDesarrollo3.Dtos
{
    namespace IntegracionDesarrollo3.Dtos
    {
        public class CreateInvoiceDTO
        {
            public int ID { get; set; }

            public int client_id { get; set; }
            public DateOnly invoice_date { get; set; }
            public decimal total_amount { get; set; }
            public int payment_method_id { get; set; }
            public DateOnly updated_at { get; set; }
        }

        public class UpdateInvoiceDTO
        {
            public int ID { get; set; }
            public int client_id { get; set; }
            public DateTime invoice_date { get; set; }
            public decimal total_amount { get; set; }
            public DateTime due_date { get; set; }
            public string status { get; set; }
        }

        public class ProductItem
        {
            public int ID { get; set; }
            // Add other product-related properties if needed
        }

        public class ServiceItem
        {
            public int ID { get; set; }
            // Add other service-related properties if needed
        }

        public class AddItemsDTO
        {
            public List<ProductItem> Products { get; set; }
            public List<ServiceItem> Services { get; set; }
            public int payment_method_id { get; set; } // Ensure this matches the type expected by the API
        }



    }

}
