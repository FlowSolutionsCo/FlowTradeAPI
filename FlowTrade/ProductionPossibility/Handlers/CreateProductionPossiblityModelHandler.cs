using FlowTrade.Exceptions;
using FlowTrade.Infrastructure.Data;
using FlowTrade.ProductionPossibility.Models;
using FlowTrade.ProductionPossibility.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;

namespace FlowTrade.ProductionPossibility.Handlers
{
    public class CreateProductionPossibilitiesModelHandler : IRequestHandler<CreateProductionPossibilityModelRequest>
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserCompany> userManager;

        public CreateProductionPossibilitiesModelHandler(AppDbContext appDbContext, UserManager<UserCompany> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }

        public async Task Handle(CreateProductionPossibilityModelRequest request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            var productionRequest = await appDbContext.ProductionPossibilities.FindAsync(new Guid(request.Model.Id.ToString()));

            if (productionRequest != null)
            {
                throw new ResourceAlreadyExistsException();
            }

            await appDbContext.ProductionPossibilities.AddAsync(request.Model, cancellationToken);

            if (user.ProductionPossibilities.IsNullOrEmpty())
            {
                user.ProductionRequestIds = $"{user.ProductionRequestIds},{request.Model.Id}";
            }
            else
            {
                user.ProductionPossibilities!.Add(await appDbContext.ProductionPossibilities.FindAsync(request.Model.Id));
            }

            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
