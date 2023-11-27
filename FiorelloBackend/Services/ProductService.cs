using AutoMapper;
using FiorelloBackend.Areas.Admin.ViewModels.Product;
using FiorelloBackend.Data;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Services
{
    public class ProductService : IProductService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductVM>> GetAllAsync()
        {
            return _mapper.Map<List<ProductVM>>(await _context.Products.Include(m => m.Category)
                                                                       .Include(m => m.Images)
                                                                       .ToListAsync());

        }

        public async Task<List<Product>> GetAllWithImagesByTakeAsync(int take)
        {
           return await _context.Products.Where(m => !m.SoftDeleted)
                                         .Include(m => m.Images)
                                         .Take(take)
                                         .ToListAsync();
        }
        

        public async Task<Product> GetByIdAsync(int id) => await _context.Products.FindAsync(id);

        public async Task<Product> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Products.Where(m => !m.SoftDeleted &&  m.Id == id)
                                                            .Include(m => m.Images)
                                                            .Include(m => m.Category)
                                                            .FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<List<ProductVM>> GetPaginatedDatasAsync(int page,int take)
        {
            List<Product> products = await _context.Products.Where(m => !m.SoftDeleted)
                                         .Include(m => m.Images)
                                         .Include(m => m.Category)
                                         .Skip((page*take)-take)
                                         .Take(take)
                                         .ToListAsync();
            return _mapper.Map<List<ProductVM>>(products);
        }
    }
}
