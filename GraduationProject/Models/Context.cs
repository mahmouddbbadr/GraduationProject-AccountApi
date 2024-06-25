using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Models
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context()
        {

        }
        public Context(DbContextOptions options) : base(options)
        {

        }
        public DbSet<XrayImage> XrayImages { get; set; }


    }
}
