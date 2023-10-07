namespace FlowTrade.Models
{
    public class ProductionPossibilityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserCompany>? Users { get; set; }
    }
}
