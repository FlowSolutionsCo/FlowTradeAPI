using FlowTrade.Exceptions;
using FlowTrade.Infrastructure.Data;
using FlowTrade.ProductionPossibility.Models;
using FlowTrade.ProductionPossibility.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto;

namespace FlowTrade.ProductionPossibility.Handlers
{
    public class DeleteProductionPossiblilityByIdHandler : IRequestHandler<DeleteProductionPossibilityByIdRequest>
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserCompany> userManager;

        public DeleteProductionPossiblilityByIdHandler(AppDbContext appDbContext, UserManager<UserCompany> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }

        public async Task Handle(DeleteProductionPossibilityByIdRequest request, CancellationToken cancellationToken)
        {
            var productionRequest = await appDbContext.ProductionRequests.FindAsync(request.RequestId);

            if (productionRequest == null)
            {
                throw new ProductionRequestNotFoundException();
            }

            if (productionRequest != null)
            {
                var user = await userManager.FindByNameAsync(productionRequest.OwnerUsername);

                if (user == null)
                {
                    throw new ProductionRequestNotFoundException();
                }

                var userRequests = user.ProductionRequestIds.Split(',');

                var updatedIds = userRequests.ToList();
                updatedIds.Remove(request.RequestId.ToString());

                user.ProductionRequestIds = string.Join(",", updatedIds);

                appDbContext.ProductionRequests.Remove(productionRequest);
                await appDbContext.SaveChangesAsync();
            }
        }
    }
}
