using System.Net.Mail;
using UserStorieCotizacion.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace UserStorieCotizacion.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void SendEmail(EmailModel emailModel)
        {
            if (string.IsNullOrEmpty(emailModel.To))
            {
                throw new ArgumentException("La dirección de correo electrónico del destinatario no puede estar vacía.", nameof(emailModel.To));
            }

            var emailMessage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress("Perdidos y Cotizaciones version 1.0", from));
            emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true; // Ignorar la validación del certificado
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    client.Authenticate(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al enviar el correo electrónico. Consulte los registros para obtener más detalles.", ex);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        // Nuevo método para enviar un mensaje de bienvenida
        public void SendWelcomeEmail(string to, string name, long cotizacionId)
        {
            var subject = "¡Bienvenido a Nuestra Plataforma!";
            var content = $@"
            <h1>Hola {name},</h1>
            <p>¡Bienvenido a nuestra plataforma! .</p>
            <p>Se acaba de aceptar tu cotización numero {cotizacionId}.</p>
            
            <p>¡Saludos!</p>";

            var emailModel = new EmailModel(to, subject, content);
            SendEmail(emailModel);
        }
    }

}
