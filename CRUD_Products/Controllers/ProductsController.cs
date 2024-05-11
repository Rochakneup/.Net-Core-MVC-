using CRUD_Products.Models;
using CRUD_Products.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CRUD_Products.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public object ModelstState { get; private set; }

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.ImagFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required.");
                return View(productDto);
            }

            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            // Construct a new file name to prevent name conflicts and to include the timestamp
            string fileName = Path.GetFileNameWithoutExtension(productDto.ImagFile.FileName);
            string extension = Path.GetExtension(productDto.ImagFile.FileName);
            string newFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

            // Ensure the directory exists
            string folderPath = Path.Combine(environment.WebRootPath, "products");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Full path for the new file
            string imagePath = Path.Combine(folderPath, newFileName);

            // Save the uploaded file to the new path
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                productDto.ImagFile.CopyTo(fileStream);
            }

            // Save the product information to the database, including the file name
            Product product = new Product
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                price = productDto.price,
                Discription = productDto.Discription,
                ImagFileName = newFileName,
                CreatedAt = DateTime.Now
            };

            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }

        public IActionResult Edit(int id)
        {

            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");

            }

            var productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                price = product.price,
                Discription = product.Discription,
            };


            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImagFileName;
            ViewData["CreateAt"] = product.CreatedAt.ToString("MM/dd/yyyy");


            return View(productDto);
        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            // Check if a new image file is uploaded
            if (productDto.ImagFile != null)
            {
                // Process the new image file
                string fileName = Path.GetFileNameWithoutExtension(productDto.ImagFile.FileName);
                string extension = Path.GetExtension(productDto.ImagFile.FileName);
                string newFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                string folderPath = Path.Combine(environment.WebRootPath, "products");
                string imagePath = Path.Combine(folderPath, newFileName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    productDto.ImagFile.CopyTo(fileStream);
                }

                // Update the product image file name in the database
                product.ImagFileName = newFileName;
            }

            // Update other product properties
            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Category = productDto.Category;
            product.price = productDto.price;
            product.Discription = productDto.Discription;

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the error or handle it as appropriate
                // Optionally, provide feedback to the user about the error
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index", "Products");
        }

        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);

               
                    if (product == null)
                {
                return RedirectToAction("Index", "Products");
            }
            
            string imageFullPath = environment.WebRootPath + "/products/"+ product.ImagFileName;
            System.IO.File.Delete(imageFullPath);
            context.Products.Remove(product);

            context.SaveChanges();
            return RedirectToAction("Index", "Products");

        }



    }
}

