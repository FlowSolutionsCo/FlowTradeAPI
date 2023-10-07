namespace FlowTrade.Exceptions
{
    public class ProductionRequestNotFoundException : ApiException
    {
        public ProductionRequestNotFoundException() : base(404, "Production request not found")
        {
        }

        public ProductionRequestNotFoundException(string message) : base(404, message)
        {
        }

        public ProductionRequestNotFoundException(string message, Exception innerException) : base(404, message, innerException)
        {
        }
    }
}
