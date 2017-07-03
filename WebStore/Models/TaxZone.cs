namespace WebStore.Models
{
    public class TaxZone
    {
        public long Id { get; set; }
        public int ZipCode { get; set; }
        public decimal TaxRate { get; set; }
    }
}
