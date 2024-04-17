﻿using System.ComponentModel.DataAnnotations;

namespace IntegracionDesarrollo3.Dtos
{
    public class LoginDTO
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string user_password { get; set; }
    }

    public class SignUpDTO
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string full_name { get; set; }

        [Required]
        public string user_password { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string phone_number { get; set; }

        public int profile_type { get; set; }
    }
}