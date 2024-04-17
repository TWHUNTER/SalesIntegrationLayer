﻿namespace IntegracionDesarrollo3.Dtos
{
    public class AccountReceivableDTO
    {
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
}
