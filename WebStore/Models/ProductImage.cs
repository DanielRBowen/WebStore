using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class ProductImage
    {
        [Key]
        [Column(Order = 0)]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Key]
        [Column(Order = 1)]
        public int ImageId { get; set; }

        public virtual Image Image { get; set; }
    }
}