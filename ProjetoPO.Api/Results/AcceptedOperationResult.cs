using Microsoft.AspNetCore.Http;

namespace ProjetoPO.Api.Results
{
    public class AcceptedOperationResult : OperationResult
    {
        private const bool DefaultSuccess = true;
        private const int DefaultStatus = StatusCodes.Status202Accepted;
        private const string DefaultMessage = "Accepted";

        public AcceptedOperationResult(string message = DefaultMessage)
            : base(DefaultSuccess, DefaultStatus, message)
        {

        }

        public AcceptedOperationResult(object data)
            : base(DefaultSuccess, DefaultStatus, DefaultMessage, data)
        {

        }

        public AcceptedOperationResult(string message, object data)
            : base(DefaultSuccess, DefaultStatus, message, data)
        {

        }
    }
}
