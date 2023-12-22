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

                WriteTXTLogs(e, context.Request);
            }
        }

        private static void WriteTXTLogs(Exception e, HttpRequest r)
        {
            var currentTime = GetCurrentTime();
            var fileName = $"{currentTime}.txt";
            var path = "/logs/";
            var fullPath = path + fileName;

            var sw = new StreamWriter(fullPath);

            sw.WriteLine(GetStreamWriterMsg("Path", r.Path.Value));
            sw.WriteLine(GetStreamWriterMsg("Method", r.Method));

            using (var reader = new StreamReader(r.Body))
            {
                string body = reader.ReadToEnd();
                sw.WriteLine(GetStreamWriterMsg("Body", body));
            }

            sw.WriteLine(GetStreamWriterMsg("Message", e.Message));
            sw.WriteLine(GetStreamWriterMsg("Source", e.Source));
            sw.WriteLine(GetStreamWriterMsg("TargetSite", e.TargetSite.ToString()));
            sw.WriteLine(GetStreamWriterMsg("StackTrace", e.StackTrace));

            sw.Close();
        }

        private static string GetStreamWriterMsg(string key, string value)
        {
            string msg = $">>>{key}:\n\t{value}\n";
            return msg;
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