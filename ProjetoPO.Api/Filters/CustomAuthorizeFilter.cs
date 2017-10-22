using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjetoPO.Api.Results;

namespace ProjetoPO.Api.Filters
{
    public class CustomAuthorizeFilter : AuthorizeFilter
    {
        public CustomAuthorizeFilter(AuthorizationPolicy policy) 
            : base(policy)
        {
        }

        public CustomAuthorizeFilter(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizeData> authorizeData) 
            : base(policyProvider, authorizeData)
        {
        }

        public CustomAuthorizeFilter(IEnumerable<IAuthorizeData> authorizeData) 
            : base(authorizeData)
        {
        }

        public CustomAuthorizeFilter(string policy) 
            : base(policy)
        {
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                return base.OnAuthorizationAsync(context);
            }

            context.Result = new UnauthorizedOperationResult("Token de acesso inválida.");
            return Task.FromResult(0);
        }
    }
}
