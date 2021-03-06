﻿using System.ComponentModel.DataAnnotations;

namespace ITNews.DTO.AccountDto
{
    public class RegistrationDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
