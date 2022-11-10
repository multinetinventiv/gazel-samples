using Gazel;

namespace Inventiv.Sample.Module.Todo;

public class TodoManager
{
    private readonly IModuleContext context;

    public TodoManager(IModuleContext context)
    {
        this.context = context;
    }

    public TaskCard CreateTaskCard(string? name)
    {
        return context.New<TaskCard>().With(name);
    }
}
