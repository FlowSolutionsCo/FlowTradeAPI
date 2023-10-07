using FlowTrade.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FlowTrade.Data
{
    public class AppDbContext : IdentityDbContext<UserCompany>
    {
        public DbSet<ProductionRequestModel> ProductionRequests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ProductionPossibilityModel> ProductionPossibilities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductionRequestModel>().HasKey(pr => pr.RequestId);
        }
    }
}