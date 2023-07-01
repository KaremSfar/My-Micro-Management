using MicroManagement.Auth.WebAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroManagement.Auth.WebAPI.Persistence
{
    public class AuthenticationServiceDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthenticationServiceDbContext(DbContextOptions<AuthenticationServiceDbContext> options)
        : base(options)
        {
        }
    }
}
