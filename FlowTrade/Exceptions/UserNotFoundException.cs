namespace FlowTrade.Exceptions
{
    public class UserNotFoundException : ApiException
    {
        public UserNotFoundException() : base(404, "User not found")
        {
        }

        public UserNotFoundException(string message) : base(404, message)
        {
        }

        public UserNotFoundException(string message, Exception innerException) : base(404, message, innerException)
        {
        }
    }
}
