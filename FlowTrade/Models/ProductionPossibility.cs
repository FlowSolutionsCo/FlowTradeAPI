namespace FlowTrade.Models
{
    public class ProductionPossibility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User>? Users { get; set; }

    }
}
