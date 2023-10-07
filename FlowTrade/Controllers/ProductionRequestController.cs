using FlowTrade.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using FlowTrade.Infrastructure.Data;
using FlowTrade.ProductionRequest.Models;
using MediatR;
using FlowTrade.ProductionRequest.Requests;
using FlowTrade.ProductionRequest.Queries;
using System.Runtime.CompilerServices;
using Azure.Security.KeyVault.Secrets;

namespace FlowTrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionRequestController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserCompany> userManager;
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public ProductionRequestController(AppDbContext appDbContext, UserManager<UserCompany> userManager, IMediator mediator, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.mediator = mediator;
            this.configuration = configuration;
        }

        [HttpPost("CreateProductionRequest")]
        public async Task<IActionResult> AddProductionRequest([FromQuery]string username, [FromBody]ProductionRequestModel model)
        {
            await this.mediator.Send(new CreateProductionRequestModelRequest(model, username));
            return Ok(model);
        }

        [HttpGet("GetProductionRequests")] 
        public async Task<IReadOnlyCollection<ProductionRequestModel>> GetProductionRequestsForUser([FromQuery]string username, [FromQuery]bool? isActive)
        {
            return await this.mediator.Send(new GetProductionRequestsForUserQuery(username, isActive));
        }

        [HttpDelete("DeleteProductionRequestById")]
        public async Task<IActionResult> DeleteRequestsForUser([FromQuery] Guid requestId)
        {
            await this.mediator.Send(new DeleteProductionRequestByIdRequest(requestId));
            return Ok();
        }
    }
}
