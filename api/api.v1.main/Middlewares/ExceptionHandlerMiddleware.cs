using component.v1.exceptions;

using System.Net;

namespace api.v1.main.Middlewares
{
    public sealed class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _request;

        public ExceptionHandlerMiddleware(RequestDelegate request) => _request = request;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (APIException e)
            {
                context.Response.StatusCode = (int)e.StatusCode;
                await context.Response.WriteAsJsonAsync(e.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync("Произошла непредвиденная ошибка. Повторите позже");

                WriteTXTLogs(e);
            }
        }

        private static void WriteTXTLogs(Exception e)
        {
            var currentTime = DateTime.UtcNow;
            var fileName = currentTime + ".txt";
            var path = "/logs/";
            var fullPath = path + fileName;

            var sw = new StreamWriter(fullPath);

            sw.WriteLine(e.Message);
            sw.WriteLine(e.Data);
            sw.WriteLine(e.Source);
            sw.WriteLine(e.TargetSite);
            sw.WriteLine(e.StackTrace);
            sw.WriteLine(e.InnerException);

            sw.Close();
        }
    }
}