using Gazel;
using Inventiv.Sample.Module.Todo.Service;

namespace Inventiv.Sample.Module.Todo;

public class TodoManager : IBoardManagerService
{
    private readonly IModuleContext _context;

    public TodoManager(IModuleContext context)
    {
        _context = context;
    }

    public Board CreateBoard(string name) => _context.New<Board>().With(name);

    IBoardInfo IBoardManagerService.CreateBoard(string name) => CreateBoard(name);
}
