using FlowTrade.Infrastructure.Data;
using FlowTrade.Interfaces;
using FlowTrade.ProductionPossibility.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowTrade.Authentication.Repositories
{
    public class ProductionPossibilityRepository : IProductionPossibilityRepository
    {
        private readonly AppDbContext _context;

        public ProductionPossibilityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductionPossibilityModel>> GetProductionPossibilitiesByIds(List<int> ids)
        {
            return await _context.ProductionPossibilities
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<List<ProductionPossibilityModel>> GetAllPossibilities()
        {
            return await _context.ProductionPossibilities.ToListAsync();
        }
    }
}
