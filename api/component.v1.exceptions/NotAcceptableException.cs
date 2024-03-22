using System.Net;

namespace component.v1.exceptions
{
    public sealed class NotAcceptableException : APIException
    {
        public NotAcceptableException(string msg) : base(HttpStatusCode.NotAcceptable, msg) { }
    }
}