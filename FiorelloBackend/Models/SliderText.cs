using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorelloBackend.Models
{
    public class SliderText:BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        
        public string Img { get; set; }
        [Required]
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
