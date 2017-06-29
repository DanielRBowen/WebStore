using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class StoreItem
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [Required]
        // To name the database schema column different than the Model: Name[Column("ItemName")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public int CatagoryId { get; set; }

        public virtual Catagory Catagory { get; set; }

        [Required]
        public string Description { get; set; }

        public int CompanyId { get; set; }

        // Navigation properties

        public Company Company { get; set; }

        public virtual ICollection<StoreItemImage> StoreItemImages { get; set; }
    }
}