using component.v1.exceptions;

using System.Net;
using System.Text;

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



        private static void WriteTXTLogs(Exception ex, HttpRequest req)
        {
            var currentTime = GetCurrentUTCTimeFormat();
            var logPath = $"/logs/{currentTime}.txt";
            using var writer = new StreamWriter(logPath);



            var requestPath = req.Path.Value;
            if (requestPath != null) 
                writer.WriteLine(GetStreamWriterMsg("Path", requestPath));

            var requestMethod = req.Method;
            writer.WriteLine(GetStreamWriterMsg("Method", requestMethod));

            var requestQueryParams = req.QueryString.ToString();
            if (requestQueryParams.Length != 0) 
                writer.WriteLine(GetStreamWriterMsg("Query params", requestQueryParams));

            var requestFormData = GetFormData(req.Form);
            if (requestFormData.Length != 0)
                writer.WriteLine(GetStreamWriterMsg("Form data", requestFormData));

            var body = GetRawBodyAsync(req).Result;
            if (body.Length != 0)
                writer.WriteLine(GetStreamWriterMsg("Body", body));

            var exceptionMsg = ex.Message;
            if (exceptionMsg.Length != 0) 
                writer.WriteLine(GetStreamWriterMsg("Message", exceptionMsg));

            var exceptionSource = ex.Source;
            if (exceptionSource != null) 
                writer.WriteLine(GetStreamWriterMsg("Source", exceptionSource));

            var exceptionTargetSite = ex.TargetSite;
            if (exceptionTargetSite != null) 
                writer.WriteLine(GetStreamWriterMsg("TargetSite", exceptionTargetSite.ToString()!));

            var exceptionStackTrace = ex.StackTrace;
            if (exceptionStackTrace != null) 
                writer.WriteLine(GetStreamWriterMsg("StackTrace", exceptionStackTrace));
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