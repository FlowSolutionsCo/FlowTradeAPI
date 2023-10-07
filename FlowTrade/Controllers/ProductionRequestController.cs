using FlowTrade.DTOs;
using FlowTrade.Helpers;
using FlowTrade.Models;
using FlowTrade.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System;
using FlowTrade.Data;
using Microsoft.IdentityModel.Tokens;

namespace FlowTrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionRequestController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserCompany> _userManager;

        public ProductionRequestController(AppDbContext appDbContext, UserManager<UserCompany> userManager)
        {
            this.appDbContext = appDbContext;
            _userManager = userManager;
        }

        [HttpPost("AddProductionRequest")]
        public async Task<IActionResult> AddProductionRequest(ProductionRequestModel model, string userId)
        {
            await appDbContext.AddAsync(model);
            var user = await _userManager.FindByNameAsync(userId);
            if(user.ProductionRequestIds.Length != 0)
            {
                user.ProductionRequestIds = $"{user.ProductionRequestIds},{model.RequestId}";
            }
            else
            {
                user.ProductionRequestIds = model.RequestId.ToString();
            }

            await appDbContext.SaveChangesAsync();  
            return Ok();    
        }

        [HttpGet("GetProductionRequest")]
        public async Task<IReadOnlyCollection<ProductionRequestModel>> GetProductionRequestsForUser(string userId)
        {
            var user = await _userManager.FindByNameAsync(userId);
            var requests = new List<ProductionRequestModel>();

            foreach (var item in user.ProductionRequestIds.Split(','))
            {
                if (!item.IsNullOrEmpty())
                {
                    requests.Add(appDbContext.ProductionRequests.Find(new Guid(item)));
                }
            }

            return requests;
        }
        [HttpDelete("DeleteRequests")]
        public async Task DeleteRequestsForUser(string userId)
        {
            var user = await _userManager.FindByNameAsync(userId);
            user.ProductionRequestIds = string.Empty;
            await appDbContext.SaveChangesAsync();
        }


    }
}
