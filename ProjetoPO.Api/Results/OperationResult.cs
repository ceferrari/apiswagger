using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProjetoPO.Api.Results
{
    public abstract class OperationResult : IActionResult
    {
        [JsonProperty("success")]
        public bool Success { get; }

        [JsonProperty("status")]
        public int Status { get; }
        
        [JsonProperty("message")]
        public string Message { get; }

        [JsonProperty("data")]
        public object Data { get; }

        protected OperationResult(bool success, int status, string message)
        {
            Success = success;
            Status = status;
            Message = message;
        }

        protected OperationResult(bool success, int status, string message, object data)
            : this(success, status, message)
        {
            Data = data;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new ObjectResult(this)
            {
                StatusCode = Status
            }.ExecuteResultAsync(context);
        }
    }
}
