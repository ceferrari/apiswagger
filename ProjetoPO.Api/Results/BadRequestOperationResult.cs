using Microsoft.AspNetCore.Http;

namespace ProjetoPO.Api.Results
{
    public class BadRequestOperationResult : OperationResult
    {
        private const bool DefaultSuccess = false;
        private const int DefaultStatus = StatusCodes.Status400BadRequest;
        private const string DefaultMessage = "Bad Request";

        public BadRequestOperationResult(string message = DefaultMessage)
            : base(DefaultSuccess, DefaultStatus, message)
        {

        }

        public BadRequestOperationResult(object data)
            : base(DefaultSuccess, DefaultStatus, DefaultMessage, data)
        {

        }

        public BadRequestOperationResult(string message, object data)
            : base(DefaultSuccess, DefaultStatus, message, data)
        {

        }
    }
}
