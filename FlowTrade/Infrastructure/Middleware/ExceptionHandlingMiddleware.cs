namespace FlowTrade.Infrastructure.Middleware
{
    using FlowTrade.Exceptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (ApiException ex)
            {
                logger.LogError(ex, "An error occurred:");

                context.Response.StatusCode = ex.StatusCode;

                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"{ex.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred:");

                context.Response.StatusCode = 500;

                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("An unexpected error occurred. Please try again later or contact support.");
            }
        }
    }
}
