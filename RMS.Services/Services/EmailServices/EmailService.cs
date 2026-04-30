using Microsoft.Extensions.Configuration;
using RMS.ServicesAbstraction.IServices.IEmailServices;
using RMS.Shared.DTOs.Utility;
using System.Net;
using System.Net.Mail;

namespace RMS.Services.Services.EmailServices
{
    public class EmailService(IConfiguration configuration) : IEmailService
    {
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(
                        configuration["EmailSettings:FromEmail"],
                        configuration["EmailSettings:AppPassword"]
                    ),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(configuration["EmailSettings:FromEmail"], SD.RestaurantName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);
                //mailMessage.ReplyToList.Add(new MailAddress(from));

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine("Email send failed: " + ex.Message);
                return false;
            }
        }
    }
}
