using FlowTrade.ProductionPossibility.Models;
using FlowTrade.ProductionRequest.Models;
using MediatR;

namespace FlowTrade.ProductionPossibility.Queries
{
    public record GetProductionPossibiliesForUserQuery(string Username, bool? isActive) : IRequest<IReadOnlyCollection<ProductionPossibilityModel>>;
}
