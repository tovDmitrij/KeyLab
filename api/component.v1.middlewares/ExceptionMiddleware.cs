using component.v1.exceptions;

using Microsoft.AspNetCore.Http;

using System.Net;
using System.Text;

namespace component.v1.middlewares
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _request;

        public ExceptionMiddleware(RequestDelegate request) => _request = request;



        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (APIException e)
            {
                context.Response.StatusCode = (int)e.StatusCode;
                await context.Response.WriteAsync(e.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("Произошла непредвиденная ошибка. Повторите позже");

                WriteTXTLogs(e, context.Request);
            }
        }



        private static void WriteTXTLogs(Exception ex, HttpRequest req)
        {
            var currentTime = GetCurrentUTCTimeFormat();
            var logPath = $"/logs/{currentTime}.txt";
            using var writer = new StreamWriter(logPath);



            try
            {
                var requestPath = req.Path.Value;
                writer.WriteLine(GetStreamWriterMsg("Path", requestPath));
            } 
            catch { }

            try
            {
                var requestMethod = req.Method;
                writer.WriteLine(GetStreamWriterMsg("Method", requestMethod));
            } 
            catch { }

            try
            {
                var requestQueryParams = req.QueryString.ToString();
                writer.WriteLine(GetStreamWriterMsg("Query params", requestQueryParams));
            }
            catch { }

            try
            {
                var requestFormData = GetFormData(req.Form);
                writer.WriteLine(GetStreamWriterMsg("Form data", requestFormData));
            } 
            catch { }

            try
            {
                var body = GetRawBodyAsync(req).Result;
                writer.WriteLine(GetStreamWriterMsg("Body", body));
            }
            catch { }

            try
            {
                var exceptionMsg = ex.Message;
                writer.WriteLine(GetStreamWriterMsg("Message", exceptionMsg));
            }
            catch { }

            try
            {
                var exceptionSource = ex.Source;
                writer.WriteLine(GetStreamWriterMsg("Source", exceptionSource));
            }
            catch { }

            try
            {
                var exceptionTargetSite = ex.TargetSite;
                writer.WriteLine(GetStreamWriterMsg("TargetSite", exceptionTargetSite.ToString()!));
            }
            catch { }

            try
            {
                var exceptionStackTrace = ex.StackTrace;
                writer.WriteLine(GetStreamWriterMsg("StackTrace", exceptionStackTrace));
            }
            catch { }
        }



        private static string GetCurrentUTCTimeFormat()
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

        private static string GetStreamWriterMsg(string key, string value) => $"\t>>>{key}<<<\n{value}\n";

        private static string GetFormData(IFormCollection formData)
        {
            var result = new StringBuilder();
            foreach (var item in formData)
            {
                result.Append($"{item.Key}: {item.Value}\n");
            }
            return result.ToString();
        }

        private static async Task<string> GetRawBodyAsync(HttpRequest req)
        {
            if (!req.Body.CanSeek) req.EnableBuffering();

            req.Body.Position = 0;
            var reader = new StreamReader(req.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);
            req.Body.Position = 0;

            return body;
        }
    }
}