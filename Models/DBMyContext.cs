using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FPTJOB.Models;

namespace FPTJOB.Models
{
    public class DBMyContext : IdentityDbContext
    {
        public DBMyContext(DbContextOptions<DBMyContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<FPTJOB.Models.Profile_Job> Profile_Job { get; set; } = default!;
    }
}
