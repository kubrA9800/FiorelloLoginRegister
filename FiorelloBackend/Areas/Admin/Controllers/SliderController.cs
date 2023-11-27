using FiorelloBackend.Areas.Admin.ViewModels.Slider;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers.Extentions;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace FiorelloBackend.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ISliderService _sliderService;
        public SliderController(AppDbContext context, IWebHostEnvironment env,ISliderService sliderService)
        {
            _context = context;
            _env = env;
            _sliderService = sliderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<SliderVM> sliders = await _sliderService.GetAllAsync();
            return View(sliders);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();


            SliderVM slider = await _sliderService.GetByIdAsync((int)id);

            if (slider is null) return NotFound();

            return View(slider);

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM slider)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            foreach (var photo in slider.Photos)
            {

                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photos", "File format can be only image");
                    return View();

                }
                if (!photo.CheckFilesize(200))
                {
                    ModelState.AddModelError("Photos", "File size can be max 200kb");
                    return View();
                }
            }
            foreach (var photo in slider.Photos) { 
            
                string fileName = $"{Guid.NewGuid()}-{photo.FileName}";
                string path = _env.GetFilePath("img", fileName);
               

                await photo.SaveFileAsync(path);

                _context.Sliders.Add(new Slider { Img=fileName});
            }
                
            

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();


            Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider is null) return NotFound();

           
            return View(new SliderEditVM { Image=slider.Img});


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,SliderEditVM slider)
        {
            if (id is null) return BadRequest();


            Slider dbSlider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (dbSlider is null) return NotFound();

            slider.Image=dbSlider.Img;

            if(slider.Photo is null){
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

            string oldPath=_env.GetFilePath("img",dbSlider.Img);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            int count=await _context.Sliders.Where(m=>m.Status).CountAsync();
            
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
            if (slider.Status)
            {
                if (count != 1)
                {
                    slider.Status = false;
                }
                else
                {
                    return RedirectToAction("Index");

                }
            }
            else
            {
                    slider.Status = true;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
           await _sliderService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


       

        


    }
}
