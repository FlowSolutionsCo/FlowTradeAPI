using FlowTrade.Exceptions;
using FlowTrade.Infrastructure.Data;
using FlowTrade.ProductionRequest.Models;
using FlowTrade.ProductionRequest.Queries;
using MediatR;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using FlowTrade.ProductionPossibility.Models;
using FlowTrade.ProductionPossibility.Queries;

namespace FlowTrade.ProductionPossibility.Handlers
{
    public class GetProductionPossibilitiesForUserHandler : IRequestHandler<GetProductionPossibiliesForUserQuery, IReadOnlyCollection<ProductionPossibilityModel>>
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserCompany> userManager;

        public GetProductionPossibilitiesForUserHandler(AppDbContext appDbContext, UserManager<UserCompany> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }

        public async Task<IReadOnlyCollection<ProductionPossibilityModel>> Handle(GetProductionPossibiliesForUserQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            var requests = new List<ProductionPossibilityModel>();

            var requestsIds = user.ProductionRequestIds.Split(',');

            foreach (var item in requestsIds)
            {
                if (item == "")
                {
                    throw new ProductionRequestNotFoundException("User doesn't have any production requests linked");
                }

                requests.Add(appDbContext.ProductionPossibilities.Find(item));
            }

            return requests;
        }
    }
}
