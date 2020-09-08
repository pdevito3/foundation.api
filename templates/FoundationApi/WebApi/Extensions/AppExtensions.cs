namespace WebApi.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using WebApi.Middleware;

    public static class AppExtensions
    {
        #region Swagger Region - Do Not Delete
        #endregion

        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
