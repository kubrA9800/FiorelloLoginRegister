using FiorelloBackend.Areas.Admin.ViewModels.Slider;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers.Extentions;
using FiorelloBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderTextController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderTextController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SliderText sliderText = await _context.SliderTexts.Where(m => !m.SoftDeleted)
                                                             .FirstOrDefaultAsync();

            return View(sliderText);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();


            SliderText sliderText = await _context.SliderTexts.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderText is null) return NotFound();

            return View(sliderText);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            SliderText sliderText = await _context.SliderTexts.FirstOrDefaultAsync(m => m.Id == id);

            _context.SliderTexts.Remove(sliderText);
            await _context.SaveChangesAsync();


            string path = _env.GetFilePath("img", sliderText.Img);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderText sliderText)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!sliderText.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photos", "File format can be only image");
                return View();

            }
            if (!sliderText.Photo.CheckFilesize(200))
            {
                ModelState.AddModelError("Photos", "File size can be max 200kb");
                return View();
            }

            
            string fileName = $"{Guid.NewGuid()}-{sliderText.Photo.FileName}";
            string path = _env.GetFilePath("img", fileName);

            sliderText.Img = fileName;
            await _context.SliderTexts.AddAsync(sliderText);

            await _context.SaveChangesAsync();
           
            await sliderText.Photo.SaveFileAsync(path);

            return RedirectToAction(nameof(Index));
        }





        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();


            SliderText sliderText = await _context.SliderTexts.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderText is null) return NotFound();

            return View(sliderText);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderText slider)
        {
            if (id is null) return BadRequest();

            SliderText dbSlider = await _context.SliderTexts.FirstOrDefaultAsync(m => m.Id == id);

            if (dbSlider is null) return NotFound();


            slider.Img = dbSlider.Img;

            ModelState.Remove("Photo");

            if (!ModelState.IsValid)
            {
                return View(slider);
            }

            if (slider.Photo is null)
            {
                dbSlider.Title = slider.Title;
                dbSlider.Description = slider.Description;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
           
            if (!slider.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File format can be only image");
                return View(slider);

            }
            if (!slider.Photo.CheckFilesize(200))
            {
                ModelState.AddModelError("Photo", "File size can be max 200kb");
                return View(slider);
            }

            string oldPath = _env.GetFilePath("img", dbSlider.Img);
            string fileName = $"{Guid.NewGuid()}-{slider.Photo.FileName}";
            string newPath = _env.GetFilePath("img", fileName);

            dbSlider.Img = fileName;
           
            await _context.SaveChangesAsync();
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            await slider.Photo.SaveFileAsync(newPath);


            return RedirectToAction("Index");
        }
    }
}
