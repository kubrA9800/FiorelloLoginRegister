using FiorelloBackend.Areas.Admin.ViewModels.Slider;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers.Extentions;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task DeleteAsync(int id)
        {
            Slider slider = await _context.Sliders.Where(m => m.Id == id).FirstOrDefaultAsync();

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();


            string path = _env.GetFilePath("img", slider.Img);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task<List<SliderVM>> GetAllAsync()
        {
             return await _context.Sliders.Select(m=>new SliderVM { Id=m.Id,Image=m.Img,Status=m.Status}).ToListAsync();
        }

        public async Task<List<SliderVM>> GetAllWithTrueStatusAsync()
        {
            return await _context.Sliders.Where(m=>m.Status).Select(m => new SliderVM { Id = m.Id, Image = m.Img, Status = m.Status }).ToListAsync();
        }

        public async Task<SliderVM> GetByIdAsync(int id)
        {
           return  await _context.Sliders.Where(m=>m.Id==id).Select(m => new SliderVM { Id = m.Id, Image = m.Img, Status = m.Status }).FirstOrDefaultAsync();
        }
    }
}
