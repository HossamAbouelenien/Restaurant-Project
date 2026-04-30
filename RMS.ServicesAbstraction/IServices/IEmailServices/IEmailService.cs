namespace RMS.ServicesAbstraction.IServices.IEmailServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);


    }
}
