using FlowTrade.Infrastructure.Data;
using FlowTrade.ProductionPossibility.Models;
using FlowTrade.ProductionPossibility.Queries;
using FlowTrade.ProductionPossibility.Requests;
using FlowTrade.ProductionRequest.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlowTrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionPossibilitiesController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserCompany> userManager;
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public ProductionPossibilitiesController(AppDbContext appDbContext, UserManager<UserCompany> userManager, IMediator mediator, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.mediator = mediator;
            this.configuration = configuration;
        }

        [HttpPost("CreateProductionPossibility")]
        public async Task<IActionResult> AddProductionPossibility([FromQuery] string username, [FromBody] ProductionPossibilityModel model)
        {
            await this.mediator.Send(new CreateProductionPossibilityModelRequest(model, username));
            return Ok(model);
        }

        [HttpGet("GetProductionPossibility")]
        public async Task<IReadOnlyCollection<ProductionPossibilityModel>> GetProductionPossibiliesForUser([FromQuery] string username, [FromQuery] bool? isActive)
        {
            return await this.mediator.Send(new GetProductionPossibiliesForUserQuery(username, isActive));
        }

        [HttpDelete("DeleteProductionPossibility")]
        public async Task<IActionResult> DeletePossibilityForUser([FromQuery] Guid requestId)
        {
            await this.mediator.Send(new DeleteProductionPossibilityByIdRequest(requestId));
            return Ok();
        }
    }
}
