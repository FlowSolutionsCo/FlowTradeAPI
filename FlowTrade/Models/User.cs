using Microsoft.AspNetCore.Identity;

namespace FlowTrade.Models
{
    public class User : IdentityUser
    {
        public string CompanyType { get; set; }
        public string AuthorizedPerson { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string Phone { get; set; }
        public ICollection<ProductionPossibility>? ProductionPossibilities { get; set; }
    }
}
