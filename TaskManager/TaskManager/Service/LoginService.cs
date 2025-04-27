using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using TaskManager.AppDbContext;

public class LoginService : ILoginService
{
    private readonly ApplicationDbContext _context;

    public LoginService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        // Generate salt
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

        // Hash the password
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: user.PasswordHash,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        user.PasswordHash = $"{Convert.ToBase64String(salt)}:{hashedPassword}";
        user.CreatedAt = DateTime.UtcNow;

        _context.Users.Add(user);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        return false;
    }

    public async Task<User> ValidateUserAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return null;

        var parts = user.PasswordHash.Split(':');
        if (parts.Length != 2)
            return null;

        var salt = Convert.FromBase64String(parts[0]);
        var storedHash = parts[1];

        var hashOfInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        if (storedHash == hashOfInput)
            return user;

        return null;
    }
}
