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
        // created_at and updated_at can be set to the current date in the backend when creating
    }

    // DTO for updating an AccountsReceivable
    public class UpdateAccountsReceivableDto
    {
        public int account_id { get; set; }
        public int? invoice_id { get; set; } // nullable in case not all fields are required for update
        public decimal? amount { get; set; } // nullable in case not all fields are required for update
        public DateOnly? due_date { get; set; } // nullable in case not all fields are required for update
        public string status { get; set; }
        // updated_at can be set to the current date in the backend when updating
    }
}
