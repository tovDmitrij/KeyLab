namespace service.v1.email.DTOs
{
    public sealed record SendEmailDTO(string EmailTo, string MsgTitle, string MsgText);
}