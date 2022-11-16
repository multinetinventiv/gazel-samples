using Gazel;
using Inventiv.Sample.Module.Todo.Service;

namespace Inventiv.Sample.Module.Todo;

public class TodoManager : IBoardManagerService
{
    private readonly IModuleContext context;

    public TodoManager(IModuleContext context)
    {
        this.context = context;
    }

    public Board CreateBoard(string name) => context.New<Board>().With(name);

    IBoardInfo IBoardManagerService.CreateBoard(string name) => CreateBoard(name);
}
