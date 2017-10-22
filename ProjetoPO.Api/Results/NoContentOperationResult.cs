using Microsoft.AspNetCore.Http;

namespace ProjetoPO.Api.Results
{
    public class NoContentOperationResult : OperationResult
    {
        private const bool DefaultSuccess = true;
        private const int DefaultStatus = StatusCodes.Status204NoContent;
        private const string DefaultMessage = "No Content";

        public NoContentOperationResult(string message = DefaultMessage)
            : base(DefaultSuccess, DefaultStatus, message)
        {

        }

        public NoContentOperationResult(object data)
            : base(DefaultSuccess, DefaultStatus, DefaultMessage, data)
        {

        }

        public NoContentOperationResult(string message, object data)
            : base(DefaultSuccess, DefaultStatus, message, data)
        {

        }
    }
}
