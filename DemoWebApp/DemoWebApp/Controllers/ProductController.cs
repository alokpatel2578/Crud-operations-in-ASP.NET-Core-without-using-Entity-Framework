using DemoWebApp.DataBaseEntity;
using DemoWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Composition.Convention;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DemoWebApp.Controllers
{

    public class ProductDTO
    {
        public List<Product> Data { get; set; }
    }
    public class ProductController : Controller
    {
        private readonly ProductDbContext _context;

        public ProductController(ProductDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Action method to get all products
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _context.Products.ToList();
            return Json(new ProductDTO { Data=products });
        }

        // Action method to display the create form
        public IActionResult Create()
        {
            return View();
        }

        // Action method to handle the form submission and create a new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // Action method to display the edit form for a specific product
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Action method to handle the form submission and update an existing product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productObj = _context.Products.Find(product.Id);
                    productObj.Name = product.Name;
                    productObj.Price = product.Price;
                    _context.Update(productObj);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }


        // Action method to delete a product
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}

