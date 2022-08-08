using System;
using Gazel;
using Gazel.Security;
using Inventiv.Todo.Module.TaskManagement.Service;

namespace Inventiv.Todo.Module.TaskManagement.Security
{
    public class SecurityManager : ISessionManager, IUserManagerService, IAuthManagerService
    {
        #region IoC

        private readonly IModuleContext context;

        public SecurityManager(IModuleContext context)
        {
            this.context = context;
        }

        #endregion

        public User CreateUser(string name, Email email, Password password) => context.New<User>().With(name, email, password);

        public UserSession Login(Email email, Password password)
        {
            var user = context.Query<Users>().SingleBy(email, password);

            if (user == null)
            {
                throw new AuthenticationRequiredException(); // This exception comes from Gazel and is a handled exception with error code 20001
            }

            return user.CreateNewSession();
        }

        private ISession GetSession(AppToken appToken)
        {
            if (appToken.IsEmpty)
            {
                return context.New<AnonymousSession>();
            }

            return context.Query<UserSessions>().SingleByToken(appToken);
        }

        #region Api Mappings

        #region Security

        ISession ISessionManager.GetSession(AppToken appToken) => GetSession(appToken);

        #endregion

        #endregion

        #region Web Service Mappings

        IUserInfo IUserManagerService.CreateUser(string name, Email email, Password password) => CreateUser(name, email, password);
        ISessionInfo IAuthManagerService.Login(Email mail, Password password) => Login(mail, password);

        #endregion
    }
}
