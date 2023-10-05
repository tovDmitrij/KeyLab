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
            var currentTime = GetCurrentTime();
            var fileName = $"{currentTime}.txt";
            var path = "/logs/";
            var fullPath = path + fileName;

            var streamWriter = new StreamWriter(fullPath);

            streamWriter.WriteLine($">>>Message:\n\t{e.Message}\n");
            streamWriter.WriteLine($">>>Data:\n\t{e.Data}\n");
            streamWriter.WriteLine($">>>Source:\n\t{e.Source}\n");
            streamWriter.WriteLine($">>>TargetSite:\n\t{e.TargetSite}\n");
            streamWriter.WriteLine($">>>StackTrace:\n{e.StackTrace}\n");

            streamWriter.Close();
        }

        private static string GetCurrentTime()
        {
            var utcNow = DateTime.UtcNow;

            var year = utcNow.Year;
            var month = utcNow.Month.ToString("00");
            var day = utcNow.Day.ToString("00");
            var hour = utcNow.Hour.ToString("00");
            var minute = utcNow.Minute.ToString("00");
            var second = utcNow.Second.ToString("00");

            var currentTime = $"{year}-{month}-{day}__{hour}-{minute}-{second}";
            return currentTime;
        }
    }
}