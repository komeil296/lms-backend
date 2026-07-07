using System.Runtime.CompilerServices;
using LMS.API.Common.Models;
using LMS.Application.Exceptions;

namespace LMS.API.Common.Middleware;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger)
    {
        _next=next;
        _logger=logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(AppException ex)
        {
            context.Response.StatusCode=ex.StatusCode;
            await context.Response.WriteAsJsonAsync(
                new ErrorResponse
                {
                    StatusCode=ex.StatusCode,
                    Message=ex.Message,
                    Timestamp=DateTime.UtcNow
                }
            );
        }
        catch(Exception ex)
        {
             _logger.LogError(ex,"Unhandled exception occured");
            await HandleExceptionAsync(context,ex);
            
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context,Exception exception)
    {
        context.Response.StatusCode=500;
        await context.Response.WriteAsJsonAsync(new ErrorResponse
        {
            StatusCode=500,
            Message="Internal server error",
            Timestamp=DateTime.UtcNow
        });
    }
}