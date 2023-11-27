using AutoMapper;
using FiorelloBackend.Areas.Admin.ViewModels.Category;
using FiorelloBackend.Data;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CategoryService(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }


        public async Task EditAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task ExtractAsync(Category category)
        {
            category.SoftDeleted = false;
            await _context.SaveChangesAsync();
        }
        public async Task<List<CategoryVM>> GetAllAsync()
        {
            return _mapper.Map<List<CategoryVM>>(await _context.Categories.ToListAsync());
        }


        public async Task<List<Category>> GetArchiveDatasAsync()
        {
          return await _context.Categories.IgnoreQueryFilters().Where(m=>m.SoftDeleted).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id, bool isTracking)
        {
            return isTracking? await _context.Categories.FirstOrDefaultAsync(m => m.Id == id): await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Category> GetByIdWithoutTrackingAsync(int id)
        {

          return await  _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(m => m.Name.Trim() == name.Trim());
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<List<CategoryVM>> GetPaginatedDatasAsync(int page, int take)
        {
            List<Category> category= await _context.Categories.Skip((page * take) - take)
                                                              .Take(take)
                                                              .ToListAsync();
            return _mapper.Map<List<CategoryVM>>(category);
        }

        public async Task<Category> GetSoftDeletedDataById(int id)
        {
          return  await _context.Categories.IgnoreQueryFilters().Where(m=>m.SoftDeleted).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task SoftDeleteAsync(Category category)
        {
            category.SoftDeleted = true;
            await _context.SaveChangesAsync();
        }

       
    }
}
