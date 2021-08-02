using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.KeyCloak
{
    public class AllowAnonymous : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext handlerContext)
        {
            foreach (var authorizationRequirement in handlerContext.PendingRequirements.ToList())
                handlerContext.Succeed(authorizationRequirement);

            return Task.CompletedTask;
        }
    }
}