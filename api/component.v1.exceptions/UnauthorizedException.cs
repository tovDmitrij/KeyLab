using System.Net;

namespace component.v1.exceptions
{
    public sealed class UnauthorizedException : APIException
    {
        public UnauthorizedException(string msg) : base(HttpStatusCode.Unauthorized, msg) { }
    }
}