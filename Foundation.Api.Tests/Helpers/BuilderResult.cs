namespace Foundation.Api.Tests.Helpers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Security.Claims;

    public class BuilderResult : HttpContextBuilderResult
    {
        public Mock<IUrlHelper> MockUrlHelper { get; set; }

        public void Deconstruct(out Mock<HttpContext> mockContext, out Mock<ClaimsIdentity> mockClaimsIdentity, out Mock<HttpRequest> mockRequest, out Mock<HttpResponse> mockResponse)
        {
            mockContext = MockContext;
            mockClaimsIdentity = MockClaimsIdentity;
            mockRequest = MockRequest;
            mockResponse = MockResponse;
        }
    }
}