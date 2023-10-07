using FlowTrade.Infrastructure.Data;
using FlowTrade.Models;
using FlowTrade.ProductionRequest.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto;

namespace FlowTrade.ProductionRequest.Handlers
{
    public class DeleteProductionRequestByIdHandler : IRequestHandler<DeleteProductionRequestByIdRequest>
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserCompany> userManager;

        public DeleteProductionRequestByIdHandler(AppDbContext appDbContext, UserManager<UserCompany> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }

        public async Task Handle(DeleteProductionRequestByIdRequest request, CancellationToken cancellationToken)
        {
            var productionRequest = await this.appDbContext.ProductionRequests.FindAsync(request.RequestId);

            if (productionRequest != null)
            {
                var user = await this.userManager.FindByNameAsync(productionRequest.OwnerUsername);
                var userRequests = user.ProductionRequestIds.Split(',');

                var updatedIds = userRequests.ToList();
                updatedIds.Remove(request.RequestId.ToString());

                user.ProductionRequestIds = string.Join(",", updatedIds);

                this.appDbContext.ProductionRequests.Remove(productionRequest);
                await appDbContext.SaveChangesAsync();
            }
        }
    }
}
