using FlowTrade.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using FlowTrade.Infrastructure.Data;
using FlowTrade.ProductionRequest.Models;
using MediatR;
using FlowTrade.ProductionRequest.Requests;
using FlowTrade.ProductionRequest.Queries;

namespace FlowTrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionRequestController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserCompany> userManager;
        private readonly IMediator mediator;

        public ProductionRequestController(AppDbContext appDbContext, UserManager<UserCompany> userManager, IMediator mediator)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.mediator = mediator;
        }

        [HttpPost("CreateProductionRequest")]
        public async Task<IActionResult> AddProductionRequest([FromQuery]string username, [FromBody]ProductionRequestModel model)
        {
            await this.mediator.Send(new CreateProductionRequestModelRequest(model, username));
            return Ok(model);
        }

        [HttpGet("GetProductionRequests")]
        public async Task<IReadOnlyCollection<ProductionRequestModel>> GetProductionRequestsForUser([FromQuery]string username)
        {
            return await this.mediator.Send(new GetProductionRequestsForUserQuery(username));
        }

        [HttpDelete("DeleteProductionRequestById")]
        public async Task<IActionResult> DeleteRequestsForUser([FromQuery] Guid requestId)
        {
            await this.mediator.Send(new DeleteProductionRequestByIdRequest(requestId));
            return Ok();
        }
    }
}
