using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class Catagory
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
