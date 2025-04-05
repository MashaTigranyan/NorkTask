using MariamApp.DTOs.Auth;

namespace MariamApp.Interfaces;

public interface IAuthService
{
    Task<string> Authenticate(string email, string password);
    
    Task InitializeDb();
}