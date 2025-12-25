using System.Text.Json;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Middlewares;

public class GlobalErrorHandlingMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalErrorHandlingMiddleWare> _logger;

    public GlobalErrorHandlingMiddleWare(RequestDelegate next,ILogger<GlobalErrorHandlingMiddleWare> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {   
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var error = new ApiException(context,exception);
        var result = JsonSerializer.Serialize(error , new JsonSerializerOptions { WriteIndented = true });
        context.Response.Clear(); 

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = error.Status!.Value;
        
        await context.Response.WriteAsync(result);
    }
}

public sealed class ApiException : ProblemDetails
{
    private HttpContext _context;
    public string? TraceId
    {
        get
        {
            if (Extensions.TryGetValue("TraceId", out var traceId))
                return traceId?.ToString();

            return _context.TraceIdentifier;
        }
        set
        {
            if (value is not null)
                Extensions["TraceId"] = value;
        }
    }
    public ApiException(HttpContext context, Exception exception)
    {
        _context = context;

        TraceId = context.TraceIdentifier;
        Instance = context.Request.Path;
        HandleException((dynamic)exception);
    }
    
    private void HandleException(NotFoundException exception)
    {
        Title = exception.GetType().Name;
        Status = StatusCodes.Status404NotFound;
        Detail = exception.Message;
    }
    private void HandleException(Exception exception)
    {
        Title = exception.GetType().Name;
        Status = StatusCodes.Status503ServiceUnavailable;
    }
}