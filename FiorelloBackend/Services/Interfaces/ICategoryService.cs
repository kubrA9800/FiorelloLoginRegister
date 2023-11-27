﻿using FiorelloBackend.Areas.Admin.ViewModels.Category;
using FiorelloBackend.Areas.Admin.ViewModels.Product;
using FiorelloBackend.Models;

namespace FiorelloBackend.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetAllAsync();
        Task<Category> GetByNameAsync(string name);
        Task<Category> GetByIdAsync(int id, bool isTracking);
        Task DeleteAsync(Category category);
        Task EditAsync(Category category);
        Task CreateAsync(Category category);
        Task<Category> GetByIdWithoutTrackingAsync(int id);
        Task SoftDeleteAsync(Category category);
        Task <List<Category>> GetArchiveDatasAsync();
        Task ExtractAsync(Category category);
        Task<Category> GetSoftDeletedDataById(int id);
        Task<List<CategoryVM>> GetPaginatedDatasAsync(int page, int take);
        Task<int> GetCountAsync();

    }
}
