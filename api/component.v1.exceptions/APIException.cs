using System.Net;

namespace component.v1.exceptions
{
    public abstract class APIException(HttpStatusCode statusCode, string msg) : Exception(msg)
    {
        public readonly HttpStatusCode StatusCode = statusCode;
    }
}