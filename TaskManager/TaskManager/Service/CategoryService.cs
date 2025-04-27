using Microsoft.EntityFrameworkCore;
using TaskManager.AppDbContext;
using TaskManager.Interface;
using TaskManager.Models;

namespace TaskManager.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context; // your DbContext

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
