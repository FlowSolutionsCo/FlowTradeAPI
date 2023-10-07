using FlowTrade.Models;

namespace FlowTrade.Interfaces
{
    public interface IProductionPossibilityRepository
    {
        Task<List<ProductionPossibilityModel>> GetProductionPossibilitiesByIds(List<int> ids);
        Task<List<ProductionPossibilityModel>> GetAllPossibilities();
    }
}
