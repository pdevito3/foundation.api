namespace Foundation.Api.Tests.Helpers
{
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;

    public class ClaimsIdentityBuilderResult
    {
        public Mock<ClaimsIdentity> MockClaimsIdentity { get; set; }
    }
}
