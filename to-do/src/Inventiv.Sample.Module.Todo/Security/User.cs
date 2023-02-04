using Gazel;
using Gazel.DataAccess;
using Gazel.Security;
using Inventiv.Sample.Module.Todo.Service;

using static Inventiv.Sample.Module.Todo.TodoExceptions;

namespace Inventiv.Sample.Module.Todo.Security;

public class User : IUserService, IUserInfo, IAccount // User implements IAccount interface so that it can be used by Gazel for authentication and authorization
{
    private readonly IRepository<User> _repository = default!;
    private readonly IModuleContext _context = default!;

    protected User() { }
    public User(IRepository<User> repository, IModuleContext context)
    {
        _repository = repository;
        _context = context;
    }

    public virtual int Id { get; protected set; }
    public virtual string Name { get; protected set; } = default!;
    public virtual Email Email { get; protected set; }
    public virtual Password Password { get; protected set; }

    protected internal virtual User With(string name, Email email, Password password)
    {
        if (string.IsNullOrWhiteSpace(name)) { throw new RequiredParameterIsMissing(nameof(name)); }
        if (email.IsDefault()) { throw new RequiredParameterIsMissing(nameof(email)); }
        if (password.IsDefault()) { throw new RequiredParameterIsMissing(nameof(password)); }

        Name = name;
        Email = email;
        Password = password;

        _repository.Insert(this);

        return this;
    }

    protected internal virtual UserSession CreateNewSession() => _context.New<UserSession>().With(this);

    public virtual List<TaskCard> GetTaskCards() => _context.Query<TaskCards>().ByUser(this);

    public virtual List<Board> GetBoards() =>
        _context.Query<UserBoards>()
            .ByUser(this)
            .Select(ub => ub.Board)
            .ToList();

    string IAccount.DisplayName => Name;
    bool IAccount.HasAccess(IResource resource) => true;

    List<ITaskCardInfo> IUserService.GetTaskCards() => GetTaskCards().ToList<ITaskCardInfo>();
    List<IBoardInfo> IUserService.GetBoards() => GetBoards().ToList<IBoardInfo>();
}

public class Users : Query<User>, IUsersService
{
    public Users(IModuleContext context) : base(context) { }

    internal User SingleBy(Email email, Password password) => SingleBy(u => u.Email == email && u.Password == password);

    internal List<User> ByName(string name) =>
        By(b => true, // default where clause always returns true so that when name is not given it will list all the users
            When(name).IsNotDefault().ThenAnd(b => b.Name.StartsWith(name)) // a short-cut for conditional query building
        );

    List<IUserInfo> IUsersService.GetUsers(string name) => ByName(name).ToList<IUserInfo>();
    IUserInfo IUsersService.GetUser(int userId) => SingleById(userId);
}
