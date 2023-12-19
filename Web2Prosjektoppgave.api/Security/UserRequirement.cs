using Microsoft.AspNetCore.Authorization;

namespace Web2Prosjektoppgave.api.Security;

public class UserRequirement : IAuthorizationRequirement
{
    public UserRequirement(int requiredUserId)
    {
        RequiredUserId = requiredUserId;
    }

    public int RequiredUserId { get; }
}