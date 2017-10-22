using Microsoft.AspNetCore.Http;

namespace ProjetoPO.Api.Results
{
    public class CreatedOperationResult : OperationResult
    {
        private const bool DefaultSuccess = true;
        private const int DefaultStatus = StatusCodes.Status201Created;
        private const string DefaultMessage = "Created";

        public CreatedOperationResult(string message = DefaultMessage)
            : base(DefaultSuccess, DefaultStatus, message)
        {

        }

        public CreatedOperationResult(object data)
            : base(DefaultSuccess, DefaultStatus, DefaultMessage, data)
        {

        }

        public CreatedOperationResult(string message, object data)
            : base(DefaultSuccess, DefaultStatus, message, data)
        {

        }
    }
}
