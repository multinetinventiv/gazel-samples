using Gazel;

namespace Inventiv.Sample.Module.Todo;

public class TodoManager
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
}
