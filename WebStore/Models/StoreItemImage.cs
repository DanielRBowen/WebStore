using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class StoreItemImage
    {
        [Key]
        [Column(Order = 0)]
        public int StoreItemId { get; set; }

        public virtual StoreItem StoreItem { get; set; }

        [Key]
        [Column(Order = 1)]
        public int ImageId { get; set; }

        public virtual Image Image { get; set; }
    }
}