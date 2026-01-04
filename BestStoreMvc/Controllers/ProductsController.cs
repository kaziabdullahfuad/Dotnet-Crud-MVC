
using System.Threading.Tasks;
using BestStoreMvc.Models;
using BestStoreMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BestStoreMvc.Controllers
{
    public class ProductsController: Controller
    {
       //private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private readonly IProductRepository _repo;
        public ProductsController(ApplicationDbContext context,IWebHostEnvironment environment, IProductRepository repo)
        {
            //this.context = context;
            this.environment = environment;
            this._repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            // var products = context.Products.ToList();
            // var products = context.Products.OrderByDescending(p => p.Id).ToList();
            // return View(products);
            var products= await _repo.GetAllAsync();
            return View(products);
        }

        // For creating a new product this method takes to the form
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            if(dto.ImageFile== null)
            {
                ModelState.AddModelError(nameof(dto.ImageFile), "The Image field is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            // ensure folder exists
            var uploads=Path.Combine(environment.WebRootPath,"products");
            if(!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            // unique filename
            var newFileName=$"{Guid.NewGuid()}{Path.GetExtension(dto.ImageFile.FileName)}";
            var fullPath=Path.Combine(uploads,newFileName);

            await using(var fs = System.IO.File.Create(fullPath))
            {
                await dto.ImageFile.CopyToAsync(fs);
            }

            var product=new Product
            {
                Name=dto.Name,
                Brand=dto.Brand,
                Category=dto.Category,
                Price=dto.Price,
                Description=dto.Description,
                ImageFileName=newFileName,
                CreatedAt=DateTime.UtcNow
            };

            await _repo.AddAsync(product);

            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var product= await _repo.GetByIdAsync(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            // create Productdto object and populate it with product data
            var productDto = new ProductDto
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,
            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");



            return View(productDto);
        }

        // POST: /Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDto dto)
        {
            var product= await _repo.GetByIdAsync(id);
            if (product == null) return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
            {
               ViewData["ProductId"]=product.Id;
               ViewData["ImageFileName"]=product.ImageFileName;
               ViewData["CreatedAt"]=product.CreatedAt.ToString("MM/dd/yyyy");
               return View(dto);
            }

            // update fields
            product.Name=dto.Name;
            product.Brand=dto.Brand;
            product.Category=dto.Category;
            product.Price=dto.Price;
            product.Description=dto.Description;

            if (dto.ImageFile != null)
            {
                var uploads=Path.Combine(environment.WebRootPath,"products");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ImageFile.FileName)}";
                var fullPath = Path.Combine(uploads, newFileName);

                await using (var fs = System.IO.File.Create(fullPath))
                {
                    await dto.ImageFile.CopyToAsync(fs);
                }
                 // optional: delete old file
                if (!string.IsNullOrEmpty(product.ImageFileName))
                {
                    var oldPath = Path.Combine(uploads, product.ImageFileName);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                product.ImageFileName = newFileName;
            }

           await _repo.UpdateAsync(product);
           return RedirectToAction(nameof(Index));
        }
        
         // DELETE action example
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product= await _repo.GetByIdAsync(id);
            if (product == null) return NotFound();

            // optionally delete file
            if (!string.IsNullOrEmpty(product.ImageFileName))
            {
                var path = Path.Combine(environment.WebRootPath, "products", product.ImageFileName);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }

            await _repo.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
           
        }
    }
}