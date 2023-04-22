using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public virtual User Username { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public bool Starred { get; set; }
    }
}
