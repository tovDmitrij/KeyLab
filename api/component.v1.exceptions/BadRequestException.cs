using System.Net;

namespace component.v1.exceptions
{
    public sealed class BadRequestException : APIException
    {
        public BadRequestException(string msg) : base(HttpStatusCode.BadRequest, msg) { }
    }
}