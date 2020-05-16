using Microsoft.AspNetCore.Http;
using Moq;

namespace Foundation.Api.Tests.Helpers
{
    public class HttpContextBuilderResult : ClaimsIdentityBuilderResult
    {
        public Mock<HttpContext> MockContext { get; set; }
        public Mock<HttpRequest> MockRequest { get; set; }
        public Mock<HttpResponse> MockResponse { get; set; }
        public Mock<IResponseCookies> MockResponseCookies { get; set; }
    }
}