using FlowTrade.Models;

namespace FlowTrade.Interfaces
{
    public interface IProductionPossibilityRepository
    {
        Task<List<ProductionPossibility>> GetProductionPossibilitiesByIds(List<int> ids);
        Task<List<ProductionPossibility>> GetAllPossibilities();
    }
}
