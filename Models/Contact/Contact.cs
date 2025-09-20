using System.ComponentModel.DataAnnotations;

namespace vipkuryen.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
        public string Title { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Telefon numrası en fazla 20 karakter olabilir.")]
        public string Phone { get; set; }

        [MaxLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
        public string Description { get; set; }
    }
}
