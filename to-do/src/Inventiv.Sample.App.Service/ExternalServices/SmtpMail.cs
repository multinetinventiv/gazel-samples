using Castle.MicroKernel;
using Gazel;
using Gazel.Configuration;
using Inventiv.Sample.Module.Todo.Mail;
using System.Net.Mail;
using System.Text;

using Component = Castle.MicroKernel.Registration.Component;

namespace Inventiv.Sample.App.Service.ExternalServices;

/// <summary>
/// Registers an smtp mail service implementation for IMailService
/// </summary>
public class SmtpMail : IIoCConfiguration
{
    public void Configure(IKernel kernel)
    {
        // Windsor registration
        kernel.Register(
         Component
                .For<IMailService>()
                .ImplementedBy<Implementation>()
                .LifestyleSingleton()
     );
    }

    /// <summary>
    /// Smtp implementation for IMailService
    /// </summary>
    public class Implementation : IMailService // TIP: When the implementation is not complex, we prefer to include it within configuration class
    {
        private readonly IModuleContext context;

        public Implementation(IModuleContext context)
        {
            this.context = context;
        }

        public void Send(string to, string subject, string content)
        {
            // Below you can see how to access web.config
            var from = context.Settings.Get<string>("SmtpMail.From");
            var smtpAddress = context.Settings.Get<string>("SmtpMail.SmtpAddress");
            var smtpPort = context.Settings.Get<int>("SmtpMail.SmtpPort");

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
