using FlowTrade.ProductionRequest.Models;
using MediatR;

namespace FlowTrade.ProductionRequest.Requests
{
    public record CreateProductionRequestModelRequest(ProductionRequestModel Model, string Username) : IRequest;
}
