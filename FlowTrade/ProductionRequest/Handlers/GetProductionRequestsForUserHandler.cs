using FlowTrade.Infrastructure.Data;
using FlowTrade.Models;
using FlowTrade.ProductionRequest.Models;
using FlowTrade.ProductionRequest.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FlowTrade.ProductionRequest.Handlers
{
    public class GetProductionRequestsForUserHandler : IRequestHandler<GetProductionRequestsForUserQuery, IReadOnlyCollection<ProductionRequestModel>>
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserCompany> userManager;

        public GetProductionRequestsForUserHandler(AppDbContext appDbContext, UserManager<UserCompany> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }

        public async Task<IReadOnlyCollection<ProductionRequestModel>> Handle(GetProductionRequestsForUserQuery request, CancellationToken cancellationToken)
        {
            var user = await this.userManager.FindByNameAsync(request.Username);
            var requests = new List<ProductionRequestModel>();

            foreach (var item in user.ProductionRequestIds.Split(','))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    requests.Add(this.appDbContext.ProductionRequests.Find(new Guid(item)));
                }
            }

            return requests;
        }
    }
}
