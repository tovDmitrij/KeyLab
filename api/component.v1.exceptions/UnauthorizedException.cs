using System.Net;

namespace component.v1.exceptions
{
    public sealed class UnauthorizedException(string msg) : APIException(HttpStatusCode.Unauthorized, msg) { }
}