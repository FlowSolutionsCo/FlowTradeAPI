using FlowTrade.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlowTrade.Models.ProductionRequestModel;

namespace FlowTrade.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<ProductionRequestModel> ProductionRequests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductionRequestModel>().HasKey(pr => pr.RequestId);

            base.OnModelCreating(modelBuilder);
        }
    }
}