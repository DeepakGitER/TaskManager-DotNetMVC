using TaskManager.Models;

namespace TaskManager.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

    }
}
