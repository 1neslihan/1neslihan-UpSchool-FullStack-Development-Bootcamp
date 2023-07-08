using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailManager : IEmailService
    {
        private readonly string _wwwrootPath;
        public EmailManager(string wwwrootPath)
        {
            _wwwrootPath = wwwrootPath;
        }

        public void SendEmailConfirmation(SendEmailConfirmationDto sendEmailConfirmationDto)
        {
            var htmlContent = File.ReadAllText($"{_wwwrootPath}/email_templates/email_confirmation.html");

            htmlContent = htmlContent.Replace("{{subject}}", MessagesHelper.Email.Confirmation.Subject);

            htmlContent = htmlContent.Replace("{{name}}", MessagesHelper.Email.Confirmation.Name(sendEmailConfirmationDto.Name));

            htmlContent = htmlContent.Replace("{{activationMessage}}", MessagesHelper.Email.Confirmation.ActivationMessage);

            htmlContent = htmlContent.Replace("{{buttonText}}", MessagesHelper.Email.Confirmation.ButtonText);

            htmlContent = htmlContent.Replace("{{buttonLink}}", MessagesHelper.Email.Confirmation.ButtonLink(sendEmailConfirmationDto.Email, sendEmailConfirmationDto.Token));

            Send(new SendEmailDto(sendEmailConfirmationDto.Email, htmlContent, MessagesHelper.Email.Confirmation.Subject));
            
        }


        private void Send (SendEmailDto sendEmailDto)
        {
            MailMessage mailMessage = new MailMessage();
            sendEmailDto.EmailAddresses.ForEach(emailAddress => mailMessage.To.Add(emailAddress));
            mailMessage.From=new MailAddress("epostaservisiuygulamasi@outlook.com");
            mailMessage.Subject=sendEmailDto.Title;
            mailMessage.IsBodyHtml=true;
            mailMessage.Body=sendEmailDto.Content;
            SmtpClient client = new SmtpClient();
            client.Port=587;
            client.Host="smtp.sendgrid.net";
            client.EnableSsl = true;
            client.UseDefaultCredentials= false;
            client.Credentials=new NetworkCredential("apikey", "");
            client.DeliveryMethod=SmtpDeliveryMethod.Network;
            client.Send(mailMessage);
        }
    }
}
