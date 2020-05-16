namespace Foundation.Api.Tests.Helpers
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;

    public interface IHttpContextBuilder<out TBuilder, out TResult> : IClaimsIdentityBuilder<TBuilder>
    {
        TBuilder WithRequest();
        TBuilder WithResponse();
        TBuilder WithContextItems(Dictionary<object, object> items);
        TBuilder WithServiceProvider(Dictionary<Type, object> services);
        //TBuilder WithCookies(Dictionary<string, string> cookies = null);
        TBuilder WithHeaders(HeaderDictionary headers);
        TBuilder WithResponseHeaders(HeaderDictionary headers);
        TBuilder WithResponseCookies();
        TResult Build();
    }
}