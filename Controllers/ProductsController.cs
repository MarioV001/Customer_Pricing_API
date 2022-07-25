using Customer_Pricing_API.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using Travelup_Products_API.Models;
using System.Linq;//"ToList" not working on "DbSet"
using Microsoft.EntityFrameworkCore;

namespace Travelup_Products_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private DBProducts_Context _dbContext;//assign dbContext from Products Context
        public ProductsController(DBProducts_Context dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetProducts")]//get list off all products and ID's
        public IActionResult Get()
        {
            //var productstest = GetProducts();//Testing  //for testing
            //return Ok(productstest);                    //for testing
            try
            {
                var Products = _dbContext.DBProducts.ToList();
                if (Products.Count == 0)
                {
                    return StatusCode(404, "No Entries Found in Data");
                }
                return Ok(Products);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error In Data");
            }
        }


        [HttpPost("AddProduct")]//add new product to DB
        public IActionResult Create([FromBody] ProductRecords request)
        {
            DBProducts Product = new DBProducts();
            Product.EAN = request.EAN;
            Product.price = request.price;

            try
            {
                _dbContext.DBProducts.Add(Product);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {

                //Code 405 had trouble with ID being NULL not saving, had to change DB to use Identity Specification
                return StatusCode(500, "Unable To Save Data" + Environment.NewLine + ex.Message);
            }
            //
            return Ok(Get());//show the new updated list

        }

        [HttpPut("UpdateProduct")]
        public IActionResult Update([FromBody] ProductRecords request)
        {
            try
            {
                var product = _dbContext.DBProducts.FirstOrDefault(x => x.ID == request.ID);
                if(product == null) return StatusCode(404, "No Product Found With That ID");
                product.EAN = request.EAN;
                product.price = request.price;
                _dbContext.Entry(product).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "Unable To Update Data");
            }
            return Ok(Get());//show the new updated list
        }

        [HttpDelete("DeleteProduct/{ID}")]
        public IActionResult Delete([FromRoute]int Id)//FromRoute = tick it from the route and pass ID
        {
            try
            {
                var product = _dbContext.DBProducts.FirstOrDefault(x => x.ID == Id);
                if (product == null) return StatusCode(404, "No Product Found With That ID");//if ID not found
                _dbContext.Entry(product).State = EntityState.Deleted;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "Unable To Delete Data");
            }
            return Ok(Get());//show the new updated list
        }

        ////testing Local
        //private List<ProductRecords> GetProducts()                           //for testing
        //{                                                                    //for testing
        //    return new List<ProductRecords>{                                 //for testing
        //        new ProductRecords { EAN = "TEST003", price = 22.22m },      //for testing
        //        new ProductRecords { EAN = "TESTEAN04", price = 16.05m }     //for testing
        //    };
        //}
    }
}
