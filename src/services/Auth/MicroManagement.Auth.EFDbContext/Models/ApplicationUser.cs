using Microsoft.AspNetCore.Identity;

namespace MicroManagement.Auth.WebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
