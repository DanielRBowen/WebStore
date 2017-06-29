using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string FileName { get; set; }

        [Required]
        [StringLength(256)]
        public string MediaType { get; set; }

        [Required]
        public byte[] Content { get; set; }
    }
}
