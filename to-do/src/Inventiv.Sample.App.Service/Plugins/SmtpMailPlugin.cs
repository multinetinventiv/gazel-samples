using Castle.MicroKernel;
using Gazel;
using Gazel.Configuration;
using Inventiv.Sample.Module.Todo.Mail;
using System.Net.Mail;
using System.Text;

using Component = Castle.MicroKernel.Registration.Component;

namespace Inventiv.Sample.App.Service.Plugins;

/// <summary>
/// Registers an smtp mail client implementation for IMailClient
/// </summary>
public class SmtpMailPlugin : IIoCConfiguration
{
    public void Configure(IKernel kernel)
    {
        // Windsor registration
        kernel.Register(
         Component
                .For<IMailClient>()
                .ImplementedBy<SmtpMailClient>()
                .LifestyleSingleton()
     );
    }

    /// <summary>
    /// Smtp mail client implementation for IMailClient
    /// </summary>
    public class SmtpMailClient : IMailClient // TIP: When the implementation is not complex, we prefer to include it within configuration class
    {
        private readonly IModuleContext _context;

        public SmtpMailClient(IModuleContext context)
        {
            _context = context;
        }

        public void Send(string to, string subject, string content)
        {
            // Below you can see how to access web.config
            var from = _context.Settings.Get<string>("SmtpMail.From");
            var smtpAddress = _context.Settings.Get<string>("SmtpMail.SmtpAddress");
            var smtpPort = _context.Settings.Get<int>("SmtpMail.SmtpPort");

            var mailMessage = new MailMessage
            {
                Subject = subject,
                Body = content,
                From = new MailAddress(from)
            };

            mailMessage.To.Add(new MailAddress(to));

            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.BodyEncoding = Encoding.UTF8;

            var client = new SmtpClient(smtpAddress, smtpPort);

            client.Send(mailMessage);
        }
    }
}
