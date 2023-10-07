using FlowTrade.ProductionRequest.Models;
using MediatR;

namespace FlowTrade.ProductionRequest.Queries
{
    public record GetProductionRequestsForUserQuery(string Username) : IRequest<IReadOnlyCollection<ProductionRequestModel>>;
}
