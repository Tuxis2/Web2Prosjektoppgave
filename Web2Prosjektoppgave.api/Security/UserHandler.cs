using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Web2Prosjektoppgave.api.Security;

public class UserHandler : AuthorizationHandler<UserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
    {
        // Assuming the user's identifier is in a claim of type "UserId"
        var userIdClaim = context.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim != null && userIdClaim.Value == requirement.RequiredUserId.ToString())
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}