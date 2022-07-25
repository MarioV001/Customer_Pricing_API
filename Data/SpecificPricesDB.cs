using System.ComponentModel.DataAnnotations;

namespace Customer_Pricing_API.Data
{
    public class SpecificPricesDB
    {
        [Key]
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public int ProductID { get; set; }//reference to original product ID
        public decimal NewPrice { get; set; }//SpecificPricesDB
    }
}
