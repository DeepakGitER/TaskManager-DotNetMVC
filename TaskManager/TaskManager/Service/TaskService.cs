using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskManager.AppDbContext;
using TaskManager.Dto;
using TaskManager.Enumerations;
using TaskManager.Interface;

namespace TaskManager.Service
{
    public class TaskService : ITaskService
    {

        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskManager.Models.Task>> GetTasksAsync()
        {
            var gettaskrecord = await _context.Tasks.Include(t => t.Category)
                                                    .Include(t => t.User).ToListAsync();
            return gettaskrecord;

        }

        public async Task<TaskManager.Models.Task> GetTaskByIdAsync(int id)
        {

            var getbyid = await _context.Tasks.FindAsync(id);

            return getbyid;
        }

        public async Task<List<TaskManager.Models.Task>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<TaskManager.Models.Task> CreateTaskAsync(TaskDto taskDto)
        {
            // Convert UserIds and CategoryIds to entities (e.g., User and Category)
            var user = _context.Users.Where(u => u.UserId == taskDto.UserId);
            var category = _context.Categories.Where(cat => cat.CategoryId == taskDto.CategoryId);

            var task = new TaskManager.Models.Task
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Priority = taskDto.Priority.ToString(),
                UserId = taskDto.UserId,
                CategoryId = taskDto.CategoryId
            };

            // Save the task in the database
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<TaskManager.Models.Task> UpdateTaskAsync(Models.Task task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskManager.Models.Task> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

            }
            return null;
        }

    }
}
