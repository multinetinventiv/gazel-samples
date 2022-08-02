using System;
using Gazel;
using Gazel.Security;

namespace Inventiv.Todo.Module.TaskManagement.Security
{
	/// <summary>
	/// Represents anonymous sessions
	/// </summary>
	public class AnonymousSession : ISession
	{
		public AnonymousSession(IModuleContext context)
		{
			Token = AppToken.Empty;
			Host = context.Request.Host.ToString(); //context.Request.Host returns client IP
			Account = context.New<AnonymousAccount>();
		}

		public AppToken Token { get; }
		public string Host { get; }
		public IAccount Account { get; }

		public void Validate() { } //this session is always valid. 
	}
}