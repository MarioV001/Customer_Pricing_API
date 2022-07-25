using System.ComponentModel.DataAnnotations;

namespace Travelup_Products_API.Models
{
    public class ProductRecords
    {
        [Key]
        public int ID { get; set; }
        public string EAN { get; set; }
        public decimal price { get; set; }
    }
}
