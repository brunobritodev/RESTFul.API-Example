using Microsoft.EntityFrameworkCore;
using RESTFul.Api.Models;

namespace RESTFul.Api.Contexts
{
    public class RestfulContext : DbContext
    {



        public RestfulContext(DbContextOptions<RestfulContext> options)
            : base(options)
        {
        }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(builder =>
            {
                builder.HasKey(o => o.Id);
                builder.HasMany(m => m.Applicants).WithOne(o => o.Company).HasForeignKey(fk => fk.CompanyId);
            });
            modelBuilder.Entity<Applicant>(builder =>
            {
                builder.HasKey(o => o.Id);
            });
        }

    }
}
