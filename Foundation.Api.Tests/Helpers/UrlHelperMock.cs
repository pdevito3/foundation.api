namespace Foundation.Api.Tests.Helpers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Moq;
    using System;

    public static class UrlHelperMock
    {
        public static Mock<IUrlHelper> Create(string returnUrl,
                                              Action<UrlActionContext> callback = null,
                                              Action<UrlRouteContext> routeCallback = null)
        {
            var urlHelperMock = new Mock<IUrlHelper>();

            urlHelperMock.Setup(
                                  x => x.Action(
                                          It.IsAny<UrlActionContext>()
                                      )
                              )
                          .Callback((UrlActionContext context) =>
                          {
                              callback?.Invoke(context);
                          })
                          .Returns(returnUrl)
                          .Verifiable();

            urlHelperMock.Setup(
                             x => x.RouteUrl(
                                 It.IsAny<UrlRouteContext>()
                             )
                         )
                         .Callback((UrlRouteContext context) =>
                         {
                             routeCallback?.Invoke(context);
                         })
                         .Returns(returnUrl)
                         .Verifiable();
            return urlHelperMock;
        }
    }
}