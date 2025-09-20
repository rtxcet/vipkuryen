using System.ComponentModel.DataAnnotations;

namespace vipkuryen.Models
{
    public class ChangeUsernameViewModel
    {
        [Required]
        [Display(Name = "Yeni Kullanıcı Adı")]
        public string NewUsername { get; set; }
    }
}
