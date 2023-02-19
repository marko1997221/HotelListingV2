using HotelListingV2.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HotelListingV2.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError($"$Something went wrong while processing {context.Request.Path}");
                await HandelExceptionAsync(context, ex);  
            }
        }

        private Task HandelExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var errordetails = new ErrorDetails
            {
                ErrorType = "Failure",
                ErrorMessage = ex.Message
            };
            switch (ex)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errordetails.ErrorType= "NotFound";
                    break;
                default:
                    break;
            }
            string resopnce = JsonConvert.SerializeObject(errordetails);
            context.Response.StatusCode=(int)statusCode;
            return context.Response.WriteAsync(resopnce);
        }
    }
}
