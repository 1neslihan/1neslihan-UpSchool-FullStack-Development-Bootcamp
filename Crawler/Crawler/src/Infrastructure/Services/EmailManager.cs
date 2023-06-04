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
        public void SendEmailConfirmation(SendEmailConfirmationDto sendEmailConfirmationDto)
        {
            throw new NotImplementedException();
            var htmlContent = $"<h4>Hi, {sendEmailConfirmationDto.Name}</h4> </br> <p>your email activation {sendEmailConfirmationDto.Link}</p>";
            var title = $"Confirm your Email Address";
            Send(new SendEmailDto(sendEmailConfirmationDto.Email, htmlContent, title));
        }


        private void Send (SendEmailDto sendEmailDto)
        {
            MailMessage mailMessage = new MailMessage();
            sendEmailDto.EmailAddresses.ForEach(emailAddress => mailMessage.To.Add(emailAddress));
            mailMessage.From=new MailAddress("noreply@entegraturk.com");
            mailMessage.Subject=sendEmailDto.Title;
            mailMessage.IsBodyHtml=true;
            mailMessage.Body=sendEmailDto.Content;
            SmtpClient client = new SmtpClient();
            client.Port=587;
            client.Host="mail.entegraturk.com";
            client.EnableSsl = false;
            client.UseDefaultCredentials= false;
            client.Credentials=new NetworkCredential("noreply@entegraturk.com", "xzx2xg4Jttrbzm5nIJ2kj1pE4l");
            client.DeliveryMethod=SmtpDeliveryMethod.Network;
            client.Send(mailMessage);
        }
    }
}
