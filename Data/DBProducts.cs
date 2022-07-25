using System.ComponentModel.DataAnnotations;

namespace Customer_Pricing_API.Data
{
    public class DBProducts
    {
        [Key]//make sure ID is the unique key
        public int ID { get; set; }
        public string EAN { get; set; }
        public decimal price { get; set; }//DBProducts
    }
}
