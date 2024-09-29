using Fantasy.shared.Responses;

namespace Fantasy.Backend.Helpers
{
    public interface IMailHelper
    {
        ActionsResponse<string> SendMail(string toName, string toEmail, string subject, string body, string language);
    }
}