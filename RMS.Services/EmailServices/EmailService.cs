using Microsoft.Extensions.Configuration;
using RMS.ServicesAbstraction.IEmailServices;
using RMS.Shared.DTOs.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.EmailServices
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
