using MediatR;

namespace FlowTrade.ProductionPossibility.Requests
{
    public record DeleteProductionPossibilityByIdRequest(Guid RequestId) : IRequest;
}
