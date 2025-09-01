using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class ApplicationUser
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "パスワードは必須です。")]
        public string Password { get; set; }
    }
}
