namespace Foundation.Api.Tests.Helpers
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public interface IClaimsIdentityBuilder<out TBuilder>
    {
        TBuilder WithUserAuthenticated(bool isAuthenticated = true);
        TBuilder WithUserClaims(List<Claim> claims = null);
        TBuilder WithUserName(string name = "TestUser");
    }
}