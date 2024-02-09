namespace service.v1.email.Service
{
    public sealed record SendEmailDTO(string EmailTo, string MsgTitle, string MsgText);
}