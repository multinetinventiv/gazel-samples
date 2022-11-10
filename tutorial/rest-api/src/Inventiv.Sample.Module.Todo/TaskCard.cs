using Gazel;
using Gazel.DataAccess;
using Inventiv.Sample.Module.Todo.Service;

namespace Inventiv.Sample.Module.Todo;

public class TaskCard : ITaskCardService, ITaskCardInfo
{
    private readonly IRepository<TaskCard> repository = default!;

    protected TaskCard() { }
    public TaskCard(IRepository<TaskCard> repository)
    {
        this.repository = repository;
    }

    public virtual int Id { get; protected set; }
    public virtual string? Name { get; protected set; }
    public virtual bool Completed { get; protected set; }

    protected internal virtual TaskCard With(string? name)
    {
        Name = name;
        Completed = false;

        repository.Insert(this);

        return this;
    }

    public virtual void Complete()
    {
        Completed = true;
    }
}

public class TaskCards : Query<TaskCard>, ITaskCardsService
{
    public TaskCards(IModuleContext context) : base(context) { }

    public List<TaskCard> ByCompleted(bool completed)
    {
        return By(t => t.Completed == completed);
    }

    ITaskCardInfo ITaskCardsService.GetTaskCard(int taskId) =>
        SingleById(taskId);

    List<ITaskCardInfo> ITaskCardsService.GetTaskCards(bool completed) =>
        ByCompleted(completed).Cast<ITaskCardInfo>().ToList();
}
