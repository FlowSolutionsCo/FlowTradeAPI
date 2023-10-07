using MediatR;

namespace FlowTrade.ProductionRequest.Requests
{
    public record DeleteProductionRequestByIdRequest(Guid RequestId) : IRequest;
}
