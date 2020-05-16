namespace Foundation.Api.Tests.Helpers
{
    using Foundation.Api.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;

    public class ControllerBuilder : IHttpContextBuilder<ControllerBuilder, BuilderResult>
    {
        private readonly Controller _controller;
        private bool _hasContext;
        private Mock<IUrlHelper> _urlHelperMock;
        private readonly HttpContextBuilder _mockHttpContextBuilder;

        public ControllerBuilder(Controller controller)
        {
            _controller = controller;

            _mockHttpContextBuilder = new HttpContextBuilder();
        }
        public ControllerBuilder WithContext()
        {
            _hasContext = true;
            return this;
        }

        public ControllerBuilder WithRequest()
        {
            _mockHttpContextBuilder.WithRequest();
            return this;
        }

        public ControllerBuilder WithContextItems(Dictionary<object, object> items)
        {
            _mockHttpContextBuilder.WithContextItems(items);
            return this;
        }

        public ControllerBuilder WithServiceProvider(Dictionary<Type, object> services)
        {
            _mockHttpContextBuilder.WithServiceProvider(services);
            return this;
        }

        public ControllerBuilder WithUrlHelper(string returnUrl = "www.google.com",
                                               Action<UrlActionContext> callback = null,
                                               Action<UrlRouteContext> routeCallback = null)
        {
            _urlHelperMock = UrlHelperMock.Create(returnUrl, callback, routeCallback);
            _controller.Url = _urlHelperMock.Object;
            return this;
        }


        public ControllerBuilder WithResponse()
        {
            _mockHttpContextBuilder.WithResponse();
            return this;
        }

        public ControllerBuilder WithUserAuthenticated(bool isAuthenticated = true)
        {
            _mockHttpContextBuilder.WithUserAuthenticated(isAuthenticated);
            return this;
        }

        public ControllerBuilder WithUserName(string name = "TestUser")
        {
            _mockHttpContextBuilder.WithUserName(name);
            return this;
        }

        public ControllerBuilder WithUserClaims(List<Claim> claims = null)
        {
            _mockHttpContextBuilder.WithUserClaims(claims);
            return this;
        }

        //public ControllerBuilder WithCookies(Dictionary<string, string> cookies = null)
        //{
        //    _mockHttpContextBuilder.WithCookies(cookies);
        //    return this;
        //}

        public ControllerBuilder WithHeaders(HeaderDictionary headers)
        {
            _mockHttpContextBuilder.WithHeaders(headers);
            return this;
        }

        public ControllerBuilder WithResponseHeaders(HeaderDictionary headers)
        {
            _mockHttpContextBuilder.WithResponseHeaders(headers);
            return this;
        }

        public ControllerBuilder WithResponseCookies()
        {
            _mockHttpContextBuilder.WithResponseCookies();
            return this;
        }

        public BuilderResult Build()
        {
            var builderResult = _mockHttpContextBuilder.Build();
            if (_hasContext)
            {
                _controller.ControllerContext = new ControllerContext { HttpContext = builderResult.MockContext.Object };
            }

            return new BuilderResult
            {
                MockContext = builderResult.MockContext,
                MockClaimsIdentity = builderResult.MockClaimsIdentity,
                MockRequest = builderResult.MockRequest,
                MockResponse = builderResult.MockResponse,
                MockUrlHelper = _urlHelperMock

            };
        }
    }
}
