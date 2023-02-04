using Gazel;
using Inventiv.Sample.Module.Todo.Service;

namespace Inventiv.Sample.Module.Todo;

public class TodoManager : ITaskCardManagerService
{
    private readonly IModuleContext _context;

    public TodoManager(IModuleContext context)
    {
        _context = context;
    }

    public TaskCard CreateTaskCard(string? name)
    {
        return _context.New<TaskCard>().With(name);
    }

    ITaskCardInfo ITaskCardManagerService.CreateTaskCard(string? name) =>
        CreateTaskCard(name);
}
