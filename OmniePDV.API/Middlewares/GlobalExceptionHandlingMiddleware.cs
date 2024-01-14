using Microsoft.AspNetCore.Mvc;
using OmniePDV.API.Exceptions;
using OmniePDV.API.Models.ViewModels.Base;
using System.Net;
using System.Text.Json;

namespace OmniePDV.API.Middlewares;

public class GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            _logger.LogInformation("{date} - Request {method} - {path}",
                DateTime.Now, context.Request.Method, context.Request.Path);
            
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{date} - {message}", DateTime.Now, e.Message);

            string json;

            if (e is BadRequestException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                json = JsonSerializer.Serialize(JsonResultViewModel<string>
                    .New(HttpStatusCode.BadRequest, e.Message));
            }
            else if (e is NotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                json = JsonSerializer.Serialize(JsonResultViewModel<string>
                    .New(HttpStatusCode.NotFound, e.Message));
            }
            else if (e is ConflictException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;

                json = JsonSerializer.Serialize(JsonResultViewModel<string>
                    .New(HttpStatusCode.Conflict, e.Message));
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails details = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = string.Format("An internal server error has occured: {0}", e.Message)
                };

                json = JsonSerializer.Serialize(details);
            }
            
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(json);
        }
    }
}
