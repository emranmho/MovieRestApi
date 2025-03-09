namespace Movies.API.Auth;

public static class AuthConstant
{
    public const string AdminUserPolicyName = "Admin";
    public const string AdminUserClaimName = "admin";
    
    public const string TrustedMemberPolicyName = "Trusted";
    public const string TrustedMemberClaimName = "trusted_member";
    
    
    public const string ApiKeyHeaderName = "x-api-key";
}