using Gazel;
using Gazel.DataAccess;
using Inventiv.Sample.Module.Todo.Mail;
using Inventiv.Sample.Module.Todo.Security;
using Inventiv.Sample.Module.Todo.Service;

using static Inventiv.Sample.Module.Todo.TodoExceptions;

namespace Inventiv.Sample.Module.Todo;

public class TaskCard : ITaskCardService, ITaskCardInfo
{
    private readonly IRepository<TaskCard> _repository = default!;
    private readonly IMailClient _mailClient = default!;
    private readonly IModuleContext _context = default!;

    protected TaskCard() { }
    public TaskCard(IRepository<TaskCard> repository, IMailClient mailClient, IModuleContext context)
    {
        _repository = repository;
        _mailClient = mailClient;
        _context = context;
    }

    public virtual int Id { get; protected set; }
    public virtual string Name { get; protected set; } = default!;
    public virtual Column Column { get; protected set; } = default!;
    public virtual string? Notes { get; protected set; }
    public virtual bool Completed { get; protected set; }
    public virtual User? User { get; protected set; }
    public virtual DateTime DueDate { get; protected set; }
    public virtual bool Reminded { get; protected set; }

    protected internal virtual TaskCard With(Column column, string name)
    {
        if (string.IsNullOrWhiteSpace(name)) { throw new RequiredParameterIsMissing(nameof(name)); }

        Column = column ?? throw new RequiredParameterIsMissing(nameof(column));
        Name = name;
        DueDate = _context.System.Now.AddDays(7);

        _repository.Insert(this);

        return this;
    }

    public virtual void Update(string? name = default, string? notes = default, DateTime dueDate = default)
    {
        if (name.IsNullOrWhiteSpace()) { name = Name; }
        if (notes.IsNullOrWhiteSpace()) { notes = Notes; }
        if (dueDate.IsDefault()) { dueDate = DueDate; }

        Name = name!;
        Notes = notes;
        DueDate = dueDate;

        if (DueDate > _context.System.Now)
        {
            Reminded = false;
        }
    }

    public virtual void Complete()
    {
        Completed = true;
        Reminded = true;
    }

    protected internal virtual void RemindUser()
    {
        if (User != null && !User.Email.IsDefault())
        {
            _mailClient.Send(
                User.Email.Value,
                $"Reminder of task: {Name}",
                $"{Name}<br/><br/>{Notes}"
            );
        }

        Reminded = true;
    }

    public virtual void Move(Column column)
    {
        Column.ValidateMovement(column);

        Column = column;
    }

    public virtual void Assign(User user)
    {
        if (user == null) { throw new RequiredParameterIsMissing(nameof(user)); }

        if (!Column.Board.HasUser(user))
        {
            throw new UserIsNotAddedToBoard(user, Column.Board);
        }

        User = user;
    }

    public virtual void Delete() => _repository.Delete(this);
}

public class TaskCards : Query<TaskCard>, ITaskCardsService
{
    public TaskCards(IModuleContext context) : base(context) { }

    internal List<TaskCard> ByColumn(Column column) => By(t => t.Column == column);
    internal List<TaskCard> ByUser(User user) => By(t => t.User == user);

    internal List<TaskCard> TakeBy(int maxRowCount, DateTime maxDueDate, bool reminded) =>
        By(t => t.DueDate < maxDueDate && t.Reminded == reminded,
            take: maxRowCount
        );

    ITaskCardInfo ITaskCardsService.GetTaskCard(int taskId) => SingleById(taskId);
}
