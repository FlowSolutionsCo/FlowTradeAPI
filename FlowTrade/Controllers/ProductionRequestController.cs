using FlowTrade.DTOs;
using FlowTrade.Helpers;
using FlowTrade.Models;
using FlowTrade.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System;
using FlowTrade.Models.ProductionRequestModel;
using FlowTrade.Data;

namespace FlowTrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionRequestController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public ProductionRequestController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpPost("AddProductionRequest")]
        public async Task<IActionResult> AddProductionRequest(ProductionRequestModel model)
        {
            await appDbContext.AddAsync(model);
            await appDbContext.SaveChangesAsync();  
            return Ok();    
        }      
    }
}
