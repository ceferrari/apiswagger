using Microsoft.AspNetCore.Http;

namespace ProjetoPO.Api.Results
{
    public class InternalServerErrorOperationResult : OperationResult
    {
        private const bool DefaultSuccess = false;
        private const int DefaultStatus = StatusCodes.Status500InternalServerError;
        private const string DefaultMessage = "Internal Server Error";

        public InternalServerErrorOperationResult(string message = DefaultMessage)
            : base(DefaultSuccess, DefaultStatus, message)
        {

        }

        public InternalServerErrorOperationResult(object data)
            : base(DefaultSuccess, DefaultStatus, DefaultMessage, data)
        {

        }

        public InternalServerErrorOperationResult(string message, object data)
            : base(DefaultSuccess, DefaultStatus, message, data)
        {

        }
    }
}
