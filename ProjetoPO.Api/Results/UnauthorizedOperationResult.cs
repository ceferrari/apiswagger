using Microsoft.AspNetCore.Http;

namespace ProjetoPO.Api.Results
{
    public class UnauthorizedOperationResult : OperationResult
    {
        private const bool DefaultSuccess = false;
        private const int DefaultStatus = StatusCodes.Status401Unauthorized;
        private const string DefaultMessage = "Unauthorized";

        public UnauthorizedOperationResult(string message = DefaultMessage)
            : base(DefaultSuccess, DefaultStatus, message)
        {
            
        }

        public UnauthorizedOperationResult(object data)
            : base(DefaultSuccess, DefaultStatus, DefaultMessage, data)
        {

        }

        public UnauthorizedOperationResult(string message, object data)
            : base(DefaultSuccess, DefaultStatus, message, data)
        {

        }
    }
}
