using FlowTrade.Data;
using FlowTrade.Interfaces;
using FlowTrade.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowTrade.Repositories
{
    public class ProductionPossibilityRepository : IProductionPossibilityRepository
    {
        private readonly AppDbContext _context;

        public ProductionPossibilityRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductionPossibility>> GetProductionPossibilitiesByIds(List<int> ids)
        {
            return await _context.ProductionPossibilities
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }
        public async Task<List<ProductionPossibility>> GetAllPossibilities()
        {
            return await _context.ProductionPossibilities.ToListAsync();
        }
    }
}
