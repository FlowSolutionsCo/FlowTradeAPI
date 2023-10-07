using FlowTrade.Exceptions;
using FlowTrade.Infrastructure.Data;
using FlowTrade.Models;
using FlowTrade.ProductionRequest.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlowTrade.ProductionRequest.Handlers
{
    public class CreateProductionRequestModelHandler : IRequestHandler<CreateProductionRequestModelRequest>
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserCompany> userManager;

        public CreateProductionRequestModelHandler(AppDbContext appDbContext, UserManager<UserCompany> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }

        public async Task Handle(CreateProductionRequestModelRequest request, CancellationToken cancellationToken)
        {
            var user = await this.userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            await this.appDbContext.AddAsync(request.Model, cancellationToken);

            if (user.ProductionRequestIds.Length != 0)
            {
                user.ProductionRequestIds = $"{user.ProductionRequestIds},{request.Model.RequestId}";
            }
            else
            {
                user.ProductionRequestIds = request.Model.RequestId.ToString();
            }

            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
