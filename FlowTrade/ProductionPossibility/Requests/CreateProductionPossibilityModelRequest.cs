using FlowTrade.ProductionPossibility.Models;
using FlowTrade.ProductionRequest.Models;
using MediatR;

namespace FlowTrade.ProductionPossibility.Requests
{
    public record CreateProductionPossibilityModelRequest(ProductionPossibilityModel Model, string Username) : IRequest;
}
