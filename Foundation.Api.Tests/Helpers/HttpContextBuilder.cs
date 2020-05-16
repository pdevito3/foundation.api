namespace Foundation.Api.Tests.Helpers
{
    using Microsoft.AspNetCore.Http;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;

    public class HttpContextBuilder : IHttpContextBuilder<HttpContextBuilder, HttpContextBuilderResult>
    {
        private readonly Mock<HttpContext> _mockContext;
        private bool _hasUser;
        private Mock<HttpRequest> _httpRequestMock;
        private Mock<HttpResponse> _httpResponseMock;
        private readonly ClaimsIdentityBuilder _mockClaimsIdentityBuilder;
        private Mock<IResponseCookies> _mockResponseCookiesCollection;

        public HttpContextBuilder()
        {
            _mockContext = new Mock<HttpContext>();
            _mockClaimsIdentityBuilder = new ClaimsIdentityBuilder();
        }

        public HttpContextBuilder WithRequest()
        {
            _httpRequestMock = new Mock<HttpRequest>(MockBehavior.Strict);
            _mockContext.SetupGet(s => s.Request).Returns(_httpRequestMock.Object);
            return this;
        }

        public HttpContextBuilder WithContextItems(Dictionary<object, object> items)
        {
            _mockContext.SetupGet(s => s.Items).Returns(items);
            return this;
        }


        public HttpContextBuilder WithResponse()
        {
            _httpResponseMock = new Mock<HttpResponse>(MockBehavior.Strict);
            _mockContext.SetupGet(s => s.Response).Returns(_httpResponseMock.Object);
            return this;
        }

        public HttpContextBuilder WithServiceProvider(Dictionary<Type, object> services)
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            foreach (var service in services)
            {
                serviceProviderMock.Setup(provider => provider.GetService(service.Key))
                                   .Returns(service.Value);

            }

            // Mock the HttpContext to return a mockable
            _mockContext.SetupGet(context => context.RequestServices)
                           .Returns(serviceProviderMock.Object);

            return this;
        }


        public HttpContextBuilder WithUserAuthenticated(bool isAuthenticated = true)
        {
            _hasUser = true;
            _mockClaimsIdentityBuilder.WithUserAuthenticated(isAuthenticated);
            return this;
        }

        public HttpContextBuilder WithUserName(string name = "TestUser")
        {
            _hasUser = true;
            _mockClaimsIdentityBuilder.WithUserName(name);
            return this;
        }

        public HttpContextBuilder WithUserClaims(List<Claim> claims = null)
        {
            _hasUser = true;
            _mockClaimsIdentityBuilder.WithUserClaims(claims);
            return this;
        }

        //public HttpContextBuilder WithCookies(Dictionary<string, string> cookies = null)
        //{
        //    if (_httpRequestMock == null)
        //    {
        //        throw new Exception("WithRequest must be called first");
        //    }

        //    if (cookies == null)
        //    {
        //        cookies = new Dictionary<string, string>();
        //    }
        //    var cc = new RequestCookieCollection(cookies);
        //    _httpRequestMock.SetupGet(s => s.Cookies).Returns(cc);
        //    return this;
        //}

        public HttpContextBuilder WithHeaders(Microsoft.AspNetCore.Http.HeaderDictionary headers)
        {
            if (_httpRequestMock == null)
            {
                throw new Exception("WithRequest must be called first");
            }
            _httpRequestMock.SetupGet(s => s.Headers).Returns(headers);
            return this;
        }

        public HttpContextBuilder WithResponseHeaders(Microsoft.AspNetCore.Http.HeaderDictionary headers)
        {
            if (_httpResponseMock == null)
            {
                throw new Exception("WithResponse must be called first");
            }
            _httpResponseMock.SetupGet(s => s.Headers).Returns(headers);
            return this;
        }

        public HttpContextBuilder WithResponseCookies()
        {
            if (_httpResponseMock == null)
            {
                throw new Exception("WithResponse must be called first");
            }
            //            var rc = new ResponseCookies(new HeaderDictionary(cookies), new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy()));
            //
            _mockResponseCookiesCollection = new Mock<IResponseCookies>();
            _httpResponseMock.SetupGet(s => s.Cookies).Returns(_mockResponseCookiesCollection.Object);
            return this;
        }

        public HttpContextBuilderResult Build()
        {
            var mockClaimsIdentity = _mockClaimsIdentityBuilder.Build();
            var user = new ClaimsPrincipal(mockClaimsIdentity.Object);
            if (_hasUser)
            {
                _mockContext.SetupGet(s => s.User).Returns(user);
            }

            return new HttpContextBuilderResult
            {
                MockContext = _mockContext,
                MockClaimsIdentity = mockClaimsIdentity,
                MockRequest = _httpRequestMock,
                MockResponse = _httpResponseMock,
                MockResponseCookies = _mockResponseCookiesCollection
            };
        }
    }
}
