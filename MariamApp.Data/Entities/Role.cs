using Microsoft.AspNetCore.Identity;

namespace MariamApp.Data.Entities;

public class Role : IdentityRole
{
    public Role()
    {
        
    }

    public Role(string role) : base(role)
    { }
}