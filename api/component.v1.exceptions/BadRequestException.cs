using System.Net;

namespace component.v1.exceptions
{
    public sealed class BadRequestException(string msg) : APIException(HttpStatusCode.BadRequest, msg) { }
}