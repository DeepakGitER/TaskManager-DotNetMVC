using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Dto;
using TaskManager.Models;
using Microsoft.AspNetCore.Authorization;

[AllowAnonymous]
public class LoginController : Controller
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var users = await _loginService.GetAllUsersAsync();
        return View(users);
    }

 
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var user = await _loginService.GetUserByIdAsync(id.Value);
        if (user == null)
            return NotFound();

        return View(user);
    }

    // GET
    public IActionResult Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Username,PasswordHash")] User user)
    {
        if (ModelState.IsValid)
        {
            var success = await _loginService.CreateUserAsync(user);

            if (success)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Unable to create user.");
        }
        return View(user);
    }

    // GET
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var user = await _loginService.GetUserByIdAsync(id.Value);
        if (user == null)
            return NotFound();

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,PasswordHash,CreatedAt")] User user)
    {
        if (id != user.UserId)
            return NotFound();

        if (ModelState.IsValid)
        {
            var success = await _loginService.UpdateUserAsync(user);

            if (success)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Unable to update user.");
        }
        return View(user);
    }

    // GET
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var user = await _loginService.GetUserByIdAsync(id.Value);
        if (user == null)
            return NotFound();

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var success = await _loginService.DeleteUserAsync(id);

        if (success)
            return RedirectToAction(nameof(Index));

        return Problem("Unable to delete user.");
    }

    public IActionResult ValidateUser()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> ValidateUser(LoginDto loginDto)
    {
        var user = await _loginService.ValidateUserAsync(loginDto.Username, loginDto.Password);
        if (user == null) return Unauthorized("Invalid credentials");

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("UserId", user.UserId.ToString())
    };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties { IsPersistent = true };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        return RedirectToAction("Index", "Home"); // Redirect after login
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _loginService.ValidateUserAsync(loginDto.Username, loginDto.Password);
        if (user == null) return Unauthorized("Invalid credentials");

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("UserId", user.UserId.ToString()),
      //  new Claim(ClaimTypes.Role, user.Role) // Optional: Role-based auth
    };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties { IsPersistent = true };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        return Ok(new { message = "Login successful" });
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        // Clear user session data
        HttpContext.Session.Clear();

        // Sign out from authentication
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home"); // Redirect to homepage after logout
    }


}
