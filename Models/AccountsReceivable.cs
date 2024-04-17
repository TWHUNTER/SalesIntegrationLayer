namespace IntegracionDesarrollo3.Models
{
    // DTO for getting a list or single AccountsReceivable
    public class AccountsReceivableModel
    {
        public int account_id { get; set; }
        public int invoice_id { get; set; }
        public decimal amount { get; set; }
        public DateOnly due_date { get; set; }
        public string status { get; set; }
        public DateOnly created_at { get; set; }
        public DateOnly updated_at { get; set; }
    }


}
