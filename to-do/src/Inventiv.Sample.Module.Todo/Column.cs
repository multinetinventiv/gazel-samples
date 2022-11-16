using Gazel;
using Gazel.DataAccess;
using Inventiv.Sample.Module.Todo.Service;

using static Inventiv.Sample.Module.Todo.TodoExceptions;

namespace Inventiv.Sample.Module.Todo;

public class Column : IColumnService, IColumnInfo, IColumnDetail
{
    private readonly IRepository<Column> repository = default!;
    private readonly IModuleContext context = default!;

    protected Column() { }
    public Column(IRepository<Column> repository, IModuleContext context)
    {
        this.repository = repository;
        this.context = context;
    }

    public virtual int Id { get; protected set; }
    public virtual string Name { get; protected set; } = default!;
    public virtual Board Board { get; protected set; } = default!;

    protected internal virtual Column With(Board board, string name)
    {
        Board = board ?? throw new RequiredParameterIsMissing(nameof(board));

        Update(name);

        repository.Insert(this);

        return this;
    }

    public virtual void Update(string name)
    {
        if (name.IsNullOrWhiteSpace()) { throw new RequiredParameterIsMissing(nameof(name)); }

        Name = name;
    }

    public virtual TaskCard AddTaskCard(string name) => context.New<TaskCard>().With(this, name);

    public virtual List<TaskCard> GetTaskCards() => context.Query<TaskCards>().ByColumn(this);

    public virtual void Delete()
    {
        Clear();

        repository.Delete(this);
    }

    public virtual void Clear() => GetTaskCards().ForEach(t => t.Delete());

    public virtual void MoveTaskCardsAndDelete(Column targetColumn)
    {
        ValidateMovement(targetColumn);

        GetTaskCards().ForEach(t => t.Move(targetColumn));

        Delete();
    }

    protected internal virtual void ValidateMovement(Column targetColumn)
    {
        if (targetColumn == null) { throw new RequiredParameterIsMissing(nameof(targetColumn)); }
        if (Board != targetColumn.Board) { throw new ColumnDoesNotBelongToBoard(targetColumn, Board); }
    }

    List<ITaskCardInfo> IColumnDetail.TaskCards => GetTaskCards().Cast<ITaskCardInfo>().ToList();

    List<ITaskCardInfo> IColumnService.GetTaskCards() => GetTaskCards().Cast<ITaskCardInfo>().ToList();
    ITaskCardInfo IColumnService.AddTaskCard(string name) => AddTaskCard(name);
}

public class Columns : Query<Column>, IColumnsService
{
    public Columns(IModuleContext context) : base(context) { }

    internal List<Column> ByBoard(Board board) => By(c => c.Board == board);

    internal List<Column> ByName(string name) => By(b => true,
            When(name).IsNotDefault().ThenAnd(b => b.Name.StartsWith(name))
        );

    IColumnDetail IColumnsService.GetColumn(int columnId) => SingleById(columnId);
}
