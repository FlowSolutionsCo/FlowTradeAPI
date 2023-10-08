using FlowTrade.Exceptions;
using FlowTrade.Infrastructure.Data;
using FlowTrade.Models;
using FlowTrade.ProductionRequest.Models;
using FlowTrade.ProductionRequest.Queries;
using MediatR;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            var requests = new List<ProductionRequestModel>();

            var requestsIds = user.ProductionRequestIds.Split(',');

            foreach (var item in requestsIds)
            {
                if (item == "")
                {
                    throw new ProductionRequestNotFoundException("User doesn't have any production requests linked");
                }

                requests.Add(this.appDbContext.ProductionRequests.Find(new Guid(item)));
            }

            if (request.isActive != null)
            {
                return requests.Where(x => x.IsActive == request.isActive).ToList();
            }

            return requests;
        }
    }
}
