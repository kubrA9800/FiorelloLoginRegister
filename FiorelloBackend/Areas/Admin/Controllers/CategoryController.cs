using FiorelloBackend.Areas.Admin.ViewModels.Category;
using FiorelloBackend.Areas.Admin.ViewModels.Product;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers;
using FiorelloBackend.Models;
using FiorelloBackend.Services;
using FiorelloBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int page=1,int take=4)
        {
            List<CategoryVM> dbPaginatedDatas = await _categoryService.GetPaginatedDatasAsync(page, take);
            int pageCount = await GetPageCount(take);
            Paginate<CategoryVM> paginatedDatas = new(dbPaginatedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        private async Task<int> GetPageCount(int take)
        {
            int categoryCount = await _categoryService.GetCountAsync();

            return (int)Math.Ceiling((decimal)categoryCount / take);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            Category existCategory = await _categoryService.GetByNameAsync(category.Name);
            if (existCategory is not null)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View();
            }
           await _categoryService.CreateAsync(category);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Category dbCategory= await _categoryService.GetByIdAsync(id,false);
            await _categoryService.DeleteAsync(dbCategory);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(int id)
        {
            Category dbCategory = await _categoryService.GetByIdAsync(id, true);
            await _categoryService.SoftDeleteAsync(dbCategory);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();


            Category category = await _categoryService.GetByIdWithoutTrackingAsync((int)id);

            if (category is null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,Category category)
        {
            if (id is null) return BadRequest();



            if (!ModelState.IsValid)
            {
                return View(category);
            }
            Category dbCategory = await _categoryService.GetByIdWithoutTrackingAsync((int) id);
            if (dbCategory is null) return NotFound();

            if (category.Name.Trim() == dbCategory.Name.Trim())
            {
                return RedirectToAction(nameof(Index));
            }
            Category existCategory = await _categoryService.GetByNameAsync(category.Name);
            if (existCategory is not null)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View();
            }

            //dbCategory.Name = category.Name;
            await _categoryService.EditAsync(category);

            return RedirectToAction("Index");
        }


    }
}
