using System.ComponentModel.DataAnnotations;

namespace ITNews.DTO.AccountDto
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
