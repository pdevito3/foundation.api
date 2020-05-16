namespace Foundation.Api.Tests.Helpers
{
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;

    public class ClaimsIdentityBuilder : IClaimsIdentityBuilder<ClaimsIdentityBuilder>
    {
        private readonly Mock<ClaimsIdentity> _mockClaimsIdentity;

        public ClaimsIdentityBuilder()
        {
            _mockClaimsIdentity = new Mock<ClaimsIdentity>();
        }

        public ClaimsIdentityBuilder WithUserAuthenticated(bool isAuthenticated = true)
        {
            _mockClaimsIdentity.SetupGet(s => s.IsAuthenticated).Returns(isAuthenticated);
            return this;
        }

        public ClaimsIdentityBuilder WithUserName(string name = "TestUser")
        {
            _mockClaimsIdentity.SetupGet(s => s.Name).Returns(name);
            return this;
        }

        public ClaimsIdentityBuilder WithUserClaims(List<Claim> claims = null)
        {
            var claimList = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, "1"),
                            };
            if (claims != null)
            {
                claimList.AddRange(claims);
            }

            _mockClaimsIdentity.SetupGet(s => s.Claims).Returns(claims);
            return this;
        }

        public Mock<ClaimsIdentity> Build()
        {
            return _mockClaimsIdentity;
        }
    }
}
