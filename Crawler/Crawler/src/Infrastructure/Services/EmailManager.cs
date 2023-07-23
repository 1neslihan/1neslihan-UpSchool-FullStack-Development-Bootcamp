using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Email;
using Domain.Entities;
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

        public void SendOrderDetails(SendOrderDetailsDto sendOrderDetailsDto)
        {
            var htmlContent = File.ReadAllText($"{_wwwrootPath}/email_templates/email_orderDetails.html");

            htmlContent=htmlContent.Replace("{{UserName}}", sendOrderDetailsDto.Name);

            htmlContent=htmlContent.Replace("{{OrderId}}", sendOrderDetailsDto.OrderId);

            htmlContent=htmlContent.Replace("{{GenerateDate}}", sendOrderDetailsDto.GenerateDate);


            StringBuilder ProductsRowsBuilder = new StringBuilder();
            foreach (var product in sendOrderDetailsDto.Products)
            {
                ProductsRowsBuilder.Append("<tr>");
                ProductsRowsBuilder.Append($"<td>{product.Name}</td>");
                ProductsRowsBuilder.Append($"<td>{product.IsOnSale}</td>");
                ProductsRowsBuilder.Append($"<td>{product.Price}</td>");
                ProductsRowsBuilder.Append($"<td>{product.SalePrice}</td>");
                ProductsRowsBuilder.Append("</tr>");
            }

            htmlContent = htmlContent.Replace("{{ProductsRows}}", ProductsRowsBuilder.ToString());

            StringBuilder OrderEventRowsBuilder = new StringBuilder();
            foreach(var orderEvent in sendOrderDetailsDto.OrderEvents)
            {
                OrderEventRowsBuilder.Append("<tr>");
                OrderEventRowsBuilder.Append($"<td>{orderEvent.Status}</td>");
                OrderEventRowsBuilder.Append($"<td>{orderEvent.OrderEventCreatedOn}</td>");
                OrderEventRowsBuilder.Append("</tr>");

            }

            htmlContent=htmlContent.Replace("{{OrderEventsRows}}", OrderEventRowsBuilder.ToString()) ;

            Send(new SendEmailDto(sendOrderDetailsDto.Email, htmlContent, "Order Details"));
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




