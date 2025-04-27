using TaskManager.Dto;

namespace TaskManager.Interface
{
    public interface ITaskService
    {
        //Task<TaskManager.Models.Task> CreateTaskAsync(Models.Task task);
        Task<TaskManager.Models.Task> CreateTaskAsync(TaskDto taskDto);
        Task<IEnumerable<TaskManager.Models.Task>> GetTasksAsync();
        Task<TaskManager.Models.Task> GetTaskByIdAsync(int id);
        Task<TaskManager.Models.Task> UpdateTaskAsync(Models.Task task);
        Task<TaskManager.Models.Task> DeleteTaskAsync(int id);
        Task<List<TaskManager.Models.Task>> GetTasksByUserIdAsync(int userId);
    }
}
