using System.ComponentModel.DataAnnotations;

namespace Travelup_Products_API.Models
{
    public class CustomerRecords
    {
        [Key]
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public int productID { get; set; }
        public decimal newPrice { get; set; }
    }
}
