using Microsoft.EntityFrameworkCore;

namespace FlowTrade.Models.ProductionRequestModel
{
    public record ProductionRequestModel
    {
        public Guid RequestId { get; set; }
        public string RequestName { get; set;}
        public double RequestAmount { get; set;}
        public bool? IsActive { get; set;}
    }
}
