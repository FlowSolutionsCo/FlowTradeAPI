using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowTrade.ProductionPossibility.Models
{
    public class UserCompany : IdentityUser
    {
        public string CompanyType { get; set; }
        public string AuthorizedPerson { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string Phone { get; set; }
        public ICollection<ProductionPossibilityModel>? ProductionPossibilities { get; set; }
        [Column("ProductionRequestIds")]
        public string ProductionRequestIds { get; set; }
    }
}
