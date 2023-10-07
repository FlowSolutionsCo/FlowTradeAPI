namespace FlowTrade.Exceptions
{
    public class ResourceAlreadyExistsException : ApiException
    {
        public ResourceAlreadyExistsException() : base(409, "Resource already exists")
        {
        }

        public ResourceAlreadyExistsException(string message) : base(409, message)
        {
        }

        public ResourceAlreadyExistsException(string message, Exception innerException) : base(409, message, innerException)
        {
        }
    }
}
