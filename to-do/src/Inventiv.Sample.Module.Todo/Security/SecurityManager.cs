using Gazel;
using Gazel.Security;
using Inventiv.Sample.Module.Todo.Service;

namespace Inventiv.Sample.Module.Todo.Security;

public class SecurityManager : ISessionManager, IUserManagerService, IAuthManagerService
{
    private readonly IModuleContext context;

    public SecurityManager(IModuleContext context)
    {
        this.context = context;
    }

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

    ISession ISessionManager.GetSession(AppToken appToken) => GetSession(appToken);

    IUserInfo IUserManagerService.CreateUser(string name, Email email, Password password) => CreateUser(name, email, password);
    ISessionInfo IAuthManagerService.Login(Email mail, Password password) => Login(mail, password);
}
