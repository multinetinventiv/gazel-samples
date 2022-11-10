using System.Net.Mail;
using System.Text;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Gazel;
using Gazel.Configuration;
using Inventiv.Todo.Module.TaskManagement.Mail;

namespace Inventiv.Todo.App.Service.ExternalServices
{
	/// <summary>
	/// Registers an smtp mail service implementation for IMailService
	/// </summary>
	public class SmtpMail : IIoCConfiguration
	{
		// TIP: When the implementation is not complex, we prefer to include it within configuration class

		public void Configure(IKernel kernel)
		{
			// Windsor registration
			kernel.Register(
				Component
					.For<IMailService>()
					.ImplementedBy<SmtpMailService>()
					.LifestyleSingleton()
			);
		}

		/// <summary>
		/// Smtp implementation for IMailService
		/// </summary>
		public class SmtpMailService : IMailService
		{
			private readonly IModuleContext context;
			public SmtpMailService(IModuleContext context)
			{
				this.context = context;
			}

			public void Send(string to, string subject, string content)
			{
				// Below you can see how to access web.config
				var from = context.Settings.Get<string>("SmtpMailService.From");
				var smtpAddress = context.Settings.Get<string>("SmtpMailService.SmtpAddress");
				var smtpPort = context.Settings.Get<int>("SmtpMailService.SmtpPort");

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
}
