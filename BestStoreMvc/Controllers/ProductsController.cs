
using BestStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace BestStoreMvc.Controllers
{
    public class ProductsController: Controller
    {
        private readonly ApplicationDbContext context;
        public ProductsController(ApplicationDbContext context)
        {
            this.context = context;
        }
       public IActionResult Index()
        {
            // var products = context.Products.ToList();
            var products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
    }
}