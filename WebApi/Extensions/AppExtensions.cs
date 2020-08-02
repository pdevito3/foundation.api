namespace WebApi.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using WebApi.Middleware;

    public static class AppExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
