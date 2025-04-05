using MariamApp.Data;
using MariamApp.Data.Entities;
using MariamApp.Interfaces;
using MariamApp.JWT;
using Microsoft.AspNetCore.Identity;

namespace MariamApp.Services;

public class AuthService : IAuthService
{
    private readonly JwtGenerator _jwtHealper; 
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly AppUsersDbContext _appUsersDbContext;
    
    public AuthService(
        JwtGenerator jwtHelper, 
        UserManager<ApplicationUser> userManager, 
        RoleManager<Role> roleManager,
        AppUsersDbContext appUsersDbContext
        )
    {
        _jwtHealper = jwtHelper;
        _userManager = userManager;
        _roleManager = roleManager;
        _appUsersDbContext = appUsersDbContext;
    }
    
    public async Task<string> Authenticate(string username, string password)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return null; 
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtHealper.GenerateToken(user.UserName, roles[0], user.Id);
        
        return token;
    }
    
    public async Task InitializeDb()
    {

        _appUsersDbContext.Database.EnsureCreated();

        if (!await _roleManager.RoleExistsAsync("Operator"))
        {
            await _roleManager.CreateAsync(new Role("Operator"));
        }
        
        if (!await _roleManager.RoleExistsAsync("Administrator"))
        {
            await _roleManager.CreateAsync(new Role("Administrator"));
        }
        
        // create admin user and assign administrator role
        var admin = await _userManager.FindByNameAsync("admin");
        
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                Email = "admin@test.com",
                PhoneNumber = "+154335345"
            };
        
            var result = await _userManager.CreateAsync(admin, "Admin040325*");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "Administrator");
            }
        }
        
        // create moderator user and assign operator role
        var moderator = await _userManager.FindByNameAsync("moderator");
        
        if (moderator == null)
        {
            moderator = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "moderator",
                Email = "moderator@test.com",
                PhoneNumber = "+345335345"
            };
        
            var result = await _userManager.CreateAsync(moderator, "Moderator040325*");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(moderator, "Operator");
            }
        }
    }
}