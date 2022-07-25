using Customer_Pricing_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Travelup_Products_API.Models;

namespace Customer_Pricing_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private DBProducts_Context _dbContext; 
        public CustomersController(DBProducts_Context dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetCustomers")]//Get list off all Customers with related product ID's
        public IActionResult Get()
        {
            try
            {
                var Customers = _dbContext.SpecificPricesDB.ToList();
                if (Customers.Count == 0)
                {
                    return StatusCode(404, "No Entries Found in Data");
                }
                return Ok(Customers);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error In Data");
            }
        }

        //Add new Customer Relation with product ID
        [HttpPost("AddCustomer")]//add new product to DB
        public IActionResult Create([FromBody] CustomerRecords request)//parameter in the Body of the request
        {
            SpecificPricesDB Customer = new SpecificPricesDB();
            Customer.CustomerName = request.CustomerName;
            Customer.ProductID = request.productID;
            if (request.newPrice.Equals(null) || request.newPrice==0) //if user does not specify new price use Default from product ID
            {
                Customer.NewPrice = GetPriceFromProdt(request.productID);//get original price
            }else Customer.NewPrice = request.newPrice;
            //lets add it
            try
            {
                _dbContext.SpecificPricesDB.Add(Customer);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //Code 405 had trouble with ID being NULL not saving, had to change DB to use Identity Specification
                return StatusCode(500, "Unable To Save Data" + Environment.NewLine + ex.Message);
            }
            //
            return Ok(Get());//show the new updated list
        }
        private decimal GetPriceFromProdt(int ProductRefID)
        {
            var product = _dbContext.DBProducts.FirstOrDefault(x => x.ID == ProductRefID);
            return product.price;
        }

        [HttpPut("UpdateCustomer")]
        public IActionResult Update([FromBody] CustomerRecords request)
        {
            try
            {
                var customer = _dbContext.SpecificPricesDB.FirstOrDefault(x => x.ID == request.ID);
                if (customer == null) return StatusCode(404, "No Product Found With That ID");
                customer.CustomerName = request.CustomerName;
                customer.ProductID = request.productID;
                customer.NewPrice = request.newPrice;
                if (request.newPrice.Equals(null) || request.newPrice == 0) //if user does not specify new price use Default from product ID
                {
                    customer.NewPrice = GetPriceFromProdt(request.productID);//get original price
                }
                else customer.NewPrice = request.newPrice;

                _dbContext.Entry(customer).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "Unable To Update Data");
            }
            return Ok(Get());//show the new updated list
        }


        [HttpDelete("DeleteProduct/{ID}")]
        public IActionResult Delete([FromRoute] int Id)//FromRoute = tick it from the route and pass ID
        {
            try
            {
                var customer = _dbContext.SpecificPricesDB.FirstOrDefault(x => x.ID == Id);
                if (customer == null) return StatusCode(404, "No Product Found With That ID");//if ID not found
                _dbContext.Entry(customer).State = EntityState.Deleted;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "Unable To Delete Data");
            }
            return Ok(Get());//show the new updated list
        }
    }
}
