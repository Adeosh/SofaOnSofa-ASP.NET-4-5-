using SofaOnSofa.Domain.Entities;
using SofaOnSofa.Domain.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace SofaOnSofa.Domain.Options
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"C:\Users\Public\Mail";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public void ProcessOrder(ShoppingCart shoppingCart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                    .AppendLine("Был отправлен новый заказ")
                    .AppendLine("___")
                    .AppendLine("Изделие: ");

                foreach (var line in shoppingCart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity, line.Product.Name, subtotal);
                }

                body.AppendFormat("Общий заказ: {0:c}", shoppingCart.ComputeTotalValue())
                    .AppendLine("___")
                    .AppendLine("Отправить в: ")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Street)
                    .AppendLine(shippingDetails.House ?? "")
                    .AppendLine(shippingDetails.Entrance ?? "")
                    .AppendLine(shippingDetails.Country)
                    .AppendLine(shippingDetails.Region ?? "")
                    .AppendLine(shippingDetails.City)
                    .AppendLine("___")
                    .AppendFormat("Платная доставка: {0}", shippingDetails.PaidDelivery ? "Да" : "Нет");

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,
                    emailSettings.MailToAddress,
                    "Заказ подтверждён!",
                    body.ToString());

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
