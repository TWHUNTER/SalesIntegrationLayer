namespace IntegracionDesarrollo3.Models
{
    // DTO for getting a list or single AccountsReceivable
    public class AccountsReceivableDto
    {
        public int account_id { get; set; }
        public int invoice_id { get; set; }
        public decimal amount { get; set; }
        public DateOnly due_date { get; set; }
        public string status { get; set; }
        public DateOnly created_at { get; set; }
        public DateOnly updated_at { get; set; }
    }

    // DTO for creating an AccountsReceivable
    public class CreateAccountsReceivableDto
    {
        public int invoice_id { get; set; }
        public decimal amount { get; set; }
        public DateOnly due_date { get; set; }
        public string status { get; set; }

    }


    public class UpdateAccountsReceivableDto
    {
        public int account_id { get; set; }
        public int? invoice_id { get; set; } 
        public decimal? amount { get; set; } 
        public DateOnly? due_date { get; set; } 
        public string status { get; set; }
        
    }
}
