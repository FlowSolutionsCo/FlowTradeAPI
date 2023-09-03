using FlowTrade.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlowTrade.Models.ProductionRequestModel;
using System.Reflection.Emit;

namespace FlowTrade.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<ProductionRequestModel> ProductionRequests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ProductionPossibility> ProductionPossibilities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductionRequestModel>().HasKey(pr => pr.RequestId);
        }
    }
}