using FiorelloBackend.Data;
using FiorelloBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            int productCount= await _context.Products.Where(m => !m.SoftDeleted)
                                                      .CountAsync();
            ViewBag.count=productCount;


            List<Product> products = await _context.Products.Where(m => !m.SoftDeleted)
                                                            .Include(m => m.Images)
                                                            .Take(4)
                                                            .ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> LoadMore(int skipCount)
        {
            List <Product> products= await _context.Products.Where(m => !m.SoftDeleted)
                                                            .Include(m => m.Images)
                                                            .Skip(skipCount)
                                                            .Take(4)
                                                            .ToListAsync();

            return PartialView("_ProductPartial", products) ;
        }

        public async Task<IActionResult> ShowLess()
        {
            List<Product> products = await _context.Products.Where(m => !m.SoftDeleted)
                                                            .Include(m => m.Images)
                                                            .Take(4)
                                                            .ToListAsync();
            return PartialView("_ProductPartial", products);
        }
    }
}
