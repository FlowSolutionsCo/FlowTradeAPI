namespace FlowTrade.Authentication.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyType { get; set; }
        public string AuthorizedPerson { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string Phone { get; set; }
        public List<int> ProductionPossibilityIds { get; set; }
    }
}
