using Microsoft.AspNetCore.Http;

namespace ProjetoPO.Api.Results
{
    public class OkOperationResult : OperationResult
    {
        private const bool DefaultSuccess = true;
        private const int DefaultStatus = StatusCodes.Status200OK;
        private const string DefaultMessage = "Ok";

        public OkOperationResult(string message = DefaultMessage)
            : base(DefaultSuccess, DefaultStatus, message)
        {

        }

        public OkOperationResult(object data)
            : base(DefaultSuccess, DefaultStatus, DefaultMessage, data)
        {

        }

        public OkOperationResult(string message, object data)
            : base(DefaultSuccess, DefaultStatus, message, data)
        {

        }
    }
}
