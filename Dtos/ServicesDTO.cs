using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;

namespace IntegracionDesarrollo3.Dtos
{
    public class ServicesDTO
    {
        public class CreateServicesDto
        {
            public string service_name { get; set; }
            public string services_description { get; set; }
            public decimal price { get; set; }
            public bool? isVisible { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        public class UpdateServicesDto
        {
            public string service_name { get; set; }
            public string services_description { get; set; }
            public decimal price { get; set; }
            public bool isVisible { get; set; }
        }


    }

}
