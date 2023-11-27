
using FiorelloBackend.Data;
using FiorelloBackend.ViewModels.Home;
using FiorelloBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using FiorelloBackend.ViewModels;
using Newtonsoft.Json;
using FiorelloBackend.Services.Interfaces;
using FiorelloBackend.Areas.Admin.ViewModels.Slider;

namespace FiorelloBackend.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly ISliderService _sliderService;
        public HomeController(AppDbContext context,
                              IProductService productService,
                              IBasketService basketService,
                              ISliderService sliderService)
        {
            _context = context;
            _productService = productService;
            _basketService = basketService;
            _sliderService = sliderService;
        }

        [HttpGet]
       
        public async Task<IActionResult> Index()
        {
            List<SliderVM> sliders = await _sliderService.GetAllWithTrueStatusAsync();
            SliderText sliderText= await _context.SliderTexts.FirstOrDefaultAsync();
            List<Blog> blogs= await _context.Blogs.Where(m=>!m.SoftDeleted).ToListAsync();
            List<Product> products = await _productService.GetAllWithImagesByTakeAsync(15);
            List<Category> categories = await _context.Categories.Where(m => !m.SoftDeleted).ToListAsync();
            About abouts= await _context.AboutContents.Where(m => !m.SoftDeleted).FirstOrDefaultAsync();
            List<HomeAboutIcon> homeAboutIcons =await _context.HomeAboutIcons.Where(m => !m.SoftDeleted).ToListAsync();
            List<Expert> experts= await _context.Experts.Where(m => !m.SoftDeleted).ToListAsync();
            HomeSubscribe homeSubscribe= await _context.HomeSubscribes.Where(m => !m.SoftDeleted).FirstOrDefaultAsync();
            List<HomeSay> homeSays= await _context.HomeSays.Where(m => !m.SoftDeleted).ToListAsync();
            List<HomeInstagram> homeInstagrams = await _context.HomeInstagrams.Where(m => !m.SoftDeleted).ToListAsync();
            HomeVm model = new()
            {
                Sliders = sliders,
                SliderTexts = sliderText,
                Blogs= blogs,
                Products= products,
                Categories= categories,
                Abouts=abouts,
                HomeAboutIcons=homeAboutIcons,
                Experts=experts,
                HomeSubscribes=homeSubscribe,
                HomeSays=homeSays,
                HomeInstagrams=homeInstagrams,
            };
            
            return View(model);
        }
        


        [HttpPost]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null) return BadRequest();

            Product product = await _productService.GetByIdAsync((int)id);

            if (product is null) return NotFound();

            _basketService.AddBasket((int)id,product);
            return Ok();
        }
        
        

 
    }
}