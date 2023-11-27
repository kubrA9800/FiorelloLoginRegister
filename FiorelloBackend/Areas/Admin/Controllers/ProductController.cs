using FiorelloBackend.Areas.Admin.ViewModels.Product;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers;
using FiorelloBackend.Helpers.Extentions;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _env;

        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, AppDbContext context, ICategoryService categoryService, IWebHostEnvironment env)
        {
            _productService = productService;  
            _context = context;
            _categoryService = categoryService;
            _env = env;
        }
        public async Task<IActionResult> Index(int page=1,int take=3)
        {
            List<ProductVM> dbPaginatedDatas = await _productService.GetPaginatedDatasAsync(page, take);
            int pageCount = await GetPageCount(take);
            Paginate<ProductVM> paginatedDatas = new(dbPaginatedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        private async Task<int> GetPageCount(int take)
        {
            int productCount = await _productService.GetCountAsync();

           return (int)Math.Ceiling((decimal)productCount / take);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Product product = await _productService.GetByIdWithIncludesAsync((int)id);
            if(product is null) return NotFound();

            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await GetCategoriesAsync(); 
            return View();
        }

        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            ViewBag.categories = await GetCategoriesAsync();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
           


            foreach (var photo in model.Photos)
            {

                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photos", "File format can be only image");
                    return View(model);

                }
                if (!photo.CheckFilesize(200))
                {
                    ModelState.AddModelError("Photos", "File size can be max 200kb");
                    return View(model);
                }
            }

            List<ProductImage> newImages = new();

            foreach (var photo in model.Photos)
            {

                string fileName = $"{Guid.NewGuid()}-{photo.FileName}";
                string path = _env.GetFilePath("img", fileName);


                await photo.SaveFileAsync(path);


                newImages.Add(new ProductImage { Image=fileName });
            }
            newImages.FirstOrDefault().IsMain = true;

            await _context.ProductImages.AddRangeAsync(newImages);

            await _context.Products.AddAsync(new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Images = newImages,
            });

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        private async Task<SelectList> GetCategoriesAsync()
        {
            return new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products.Include(m=>m.Images).FirstOrDefaultAsync(m=>m.Id==id);

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            foreach (var photo in product.Images)
            {
                
                    string path = _env.GetFilePath("img", photo.Image);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
           
               
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.categories = await GetCategoriesAsync();

            if (id == null) return BadRequest();

            Product product = await _productService.GetByIdWithIncludesAsync((int)id);

            if(product == null) return NotFound();


            return View(new ProductEditVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = (int)product.CategoryId,
                Price = product.Price,
                Images = product.Images.ToList()
            }); 


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,ProductEditVM request)
        {
            ViewBag.categories = await GetCategoriesAsync();

            if (id == null) return BadRequest();

            Product product = await _productService.GetByIdWithIncludesAsync((int)id);

            if (product == null) return NotFound();

            request.Images= product.Images; 

            if(!ModelState.IsValid)
            {
                return View(request);
            }

       
            List<ProductImage> newImages = new();

            if (request.Photos != null)
            {
                foreach (var photo in request.Photos)
                {

                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photos", "File format can be only image");
                        return View(request);

                    }
                    if (!photo.CheckFilesize(200))
                    {
                        ModelState.AddModelError("Photos", "File size can be max 200kb");
                        return View(request);
                    }
                }

                foreach (var photo in request.Photos)
                {

                    string fileName = $"{Guid.NewGuid()}-{photo.FileName}";
                    string path = _env.GetFilePath("img", fileName);


                    await photo.SaveFileAsync(path);


                    newImages.Add(new ProductImage { Image = fileName });
                }
                await _context.ProductImages.AddRangeAsync(newImages);

            }
            newImages.AddRange(request.Images);

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price= request.Price;
            product.CategoryId= request.CategoryId;
            product.Images= newImages;


            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }


        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            ProductImage image = await _context.ProductImages.Where(m => m.Id == id).FirstOrDefaultAsync();

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();


            string path = _env.GetFilePath("img", image.Image);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return Ok();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeMainImage(int id)
        {
            int count = await _context.ProductImages.Where(m => m.IsMain).CountAsync();

            ProductImage image =_context.ProductImages.FirstOrDefault(m => m.Id == id);

            if (image.IsMain)
            {
               
                    image.IsMain = false;
        
            }
            else
            {

                image.IsMain = true;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
