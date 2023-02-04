using Gazel;
using Gazel.DataAccess;

namespace Inventiv.Sample.Module.Todo;

public class TaskCard
{
    private readonly IRepository<TaskCard> _repository = default!;

    protected TaskCard() { }
    public TaskCard(IRepository<TaskCard> repository)
    {
        _repository = repository;
    }

    public virtual int Id { get; protected set; } 
    public virtual string? Name { get; protected set; }
    public virtual bool Completed { get; protected set; } 

    protected internal virtual TaskCard With(string? name)
    {
        Name = name;
        Completed = false;

        _repository.Insert(this);

        return this;
    }

    public virtual void Complete()
    {
        Completed = true;
    }
}

public class TaskCards : Query<TaskCard>
{
    public TaskCards(IModuleContext context) : base(context) { }

    public List<TaskCard> ByCompleted(bool completed)
    {
        return By(t => t.Completed == completed);
    }
}
