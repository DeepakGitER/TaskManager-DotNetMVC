using TaskManager.Models;

public interface ILoginService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<bool> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<User> ValidateUserAsync(string username, string password);
}
