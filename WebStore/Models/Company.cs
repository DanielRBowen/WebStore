using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Company Name")]
        [StringLength(50, ErrorMessage = "Company Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [DataType(DataType.Url)]
        public string Website { get; set; }

        // Navigation properties

        public ICollection<StoreItem> StoreItems { get; set; }
    }
}
