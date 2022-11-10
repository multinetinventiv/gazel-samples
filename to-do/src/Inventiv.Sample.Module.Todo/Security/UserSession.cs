using Gazel;
using Gazel.DataAccess;
using Gazel.Security;
using Inventiv.Sample.Module.Todo.Service;

namespace Inventiv.Sample.Module.Todo.Security;

public class UserSession : IAuditable, ISession, ISessionInfo
{
    private readonly IRepository<UserSession> repository = default!;
    private readonly IModuleContext context = default!;

    protected UserSession() { }
    public UserSession(IRepository<UserSession> repository, IModuleContext context)
    {
        this.repository = repository;
        this.context = context;
    }

    public virtual int Id { get; protected set; }
    public virtual User User { get; protected set; } = default!;
    public virtual AppToken Token { get; protected set; }
    public virtual DateTime ExpireDateTime { get; protected set; }
    public virtual string Host { get; protected set; } = default!;
    public virtual AuditInfo AuditInfo { get; protected set; }

    protected internal virtual UserSession With(User user)
    {
        User = user;

        // context.System makes it possible to mock new guid & app token generations and system time.
        Token = context.System.NewAppToken();
        ExpireDateTime = context.System.Now.AddDays(1);
        Host = context.Request.Host.ToString();

        repository.Insert(this);

        return this;
    }

    protected internal virtual void Validate()
    {
        if (context.System.Now > ExpireDateTime)
        {
            throw new AuthenticationRequiredException(); // This exception comes from Gazel and is a handled exception with error code 20001
        }
    }

    IAccount ISession.Account => User;
    void ISession.Validate() => Validate();

    IUserInfo ISessionInfo.User => User; // Return type of actual property is User, but ISessionInfo requires it to be IUserInfo
}

public class UserSessions : Query<UserSession>
{
    public UserSessions(IModuleContext context) : base(context) { }

    internal UserSession SingleByToken(AppToken token) => SingleBy(s => s.Token == token);
}
