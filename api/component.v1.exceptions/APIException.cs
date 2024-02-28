using System.Net;

namespace component.v1.exceptions
{
    public abstract class APIException : Exception
    {
        public readonly HttpStatusCode StatusCode;

        public APIException(HttpStatusCode statusCode, string msg) : base(msg) => StatusCode = statusCode;
    }
}