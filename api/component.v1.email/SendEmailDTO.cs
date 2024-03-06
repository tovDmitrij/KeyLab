namespace component.v1.email
{
    public sealed record SendEmailDTO(string EmailTo, string MsgTitle, string MsgText);
}