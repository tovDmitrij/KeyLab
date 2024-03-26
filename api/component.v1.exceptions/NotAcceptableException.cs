using System.Net;

namespace component.v1.exceptions
{
    public sealed class NotAcceptableException(string msg) : APIException(HttpStatusCode.NotAcceptable, msg) { }
}