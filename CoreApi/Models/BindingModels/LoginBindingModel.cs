using System.ComponentModel.DataAnnotations;

namespace CoreApi.Models.BindingModels
{
    public class LoginBindingModel
    {
        [Required]
        [Display(Name = "username")]
        public string username { get; set; }

        [Required]
        [Display(Name = "password")]
        public string password { get; set; }
    }

    public class RefreshTokenBindingModel
    {
        [Required]
        [Display(Name = "token")]
        public string token { get; set; }

        [Required]
        [Display(Name = "refreshToken")]
        public string refreshToken { get; set; }
    }

    public class CheckTokenBindingModel
    {
        [Required]
        [Display(Name = "token")]
        public string token { get; set; }
    }
}
