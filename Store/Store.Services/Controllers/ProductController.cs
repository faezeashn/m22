using Store.Models.Models;
using Store.Services.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Store.Services.Controllers
{
    public class ProductController : ApiController
    {
        private Storecontext _db = new Storecontext();

     //گرفتن همه محصولات
        [HttpGet]
        public IHttpActionResult GettingAllProduct()
        {
            var products = _db.Product.AsNoTracking()
                .Select(f => new Products
                {
                    Id = f.Id,
                    CategoryName = f.Category.Name,
                   
                    CompanyName = f.Company.Name,
                    ModelNo = f.ModelNo,
                    BrandName = f.BrandName,

                }).ToList();

           
            return Ok(products);
        }

        // GET: api/Products/5
        [ResponseType(typeof(Products))]
        [HttpGet]
        //گرفتن یک محصول
        public IHttpActionResult GettsProduct([FromUri]int id)
        {
            var product = _db.Product.
                Where( s=>s.Id == id)
                .Select(s => new
                {
                    Id = s.Id,
                    CompanyName = s.Company.Name,
                    CategoryName = s.Category.Name,
                    ModelNo = s.ModelNo,
                    BrandName = s.BrandName,
                    
                    
                   

                }).SingleOrDefault();

            if (product == null)
            {
                return NotFound();
            }


            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult EdittProduct(int id, Products product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            _db.Entry(product).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Products))]
        [HttpPost]
        public IHttpActionResult AddProduct([FromBody]Products product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Product.Add(product);
            _db.SaveChanges();

            return Ok("200");
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Products))]
        [HttpDelete]
        public IHttpActionResult DeletProduct([FromUri]int id)
        {
            var product = _db.Product.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            _db.Product.Remove(product);
            _db.SaveChanges();

            return Ok("200");
        }



        [HttpGet]
        public IHttpActionResult GetCountries()
        {
            var countries = _db.Countries;

            List<Country> countrie = new List<Country>();
            foreach (var country in countries)
            {
                Country model = new Country()
                {

                    Id = country.Id,
                    Name = country.Name

                };
                countrie.Add(model);
            }
            return Ok(countrie);
        }


        [HttpGet]
        public IHttpActionResult GetCategories()
        {

            var categories = _db.Categories.ToList();

            List<Category> categorie = new List<Category>();
            foreach (var category in categories)
            {
                Category model = new Category()
                {

                    Id = category.Id,
                    Name = category.Name,

                };
                categorie.Add(model);
            }

            return Ok(categorie);
        }

        [HttpGet]
        public IHttpActionResult GetComponies()
        {
            var companies = _db.Companies.AsNoTracking()
                       .Select(c => new Company
                       {
                           Id = c.Id,
                           Name = c.Name

                       }).ToList();

            return Ok(companies);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return _db.Product.Count(e => e.Id == id) > 0;
        }
    }
}
