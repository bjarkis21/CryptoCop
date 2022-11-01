using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Cryptocop.Software.API.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = exceptionHandlerFeature.Error;
                    Console.WriteLine(exception.StackTrace);
                    var statusCode = (int)HttpStatusCode.InternalServerError;
                    
                    if (exception is ModelFormatException)
                    {
                        statusCode = (int)HttpStatusCode.PreconditionFailed;
                    }
                    else if (exception is UnauthorizedException)
                    {
                        statusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else if (exception is ProductException)
                    {
                        statusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if (exception is CartItemException)
                    {
                        statusCode = (int)HttpStatusCode.NotFound;
                    }
                    else if (exception is OrderException)
                    {
                        statusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if (exception is ResourceNotFoundException)
                    {
                        statusCode = (int)HttpStatusCode.NotFound;
                    }
                    else
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError;
                    }
                    

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = statusCode;

                    await context.Response.WriteAsync(new ExceptionModel
                    {
                        StatusCode = statusCode,
                        Message = exception.Message
                    }.ToString()); 
                });
            });
        }
    }
}