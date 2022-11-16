using Gazel;
using Inventiv.Sample.Module.Todo.Service;

namespace Inventiv.Sample.Module.Todo;

public class TodoManager : ITaskCardManagerService
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

    ITaskCardInfo ITaskCardManagerService.CreateTaskCard(string? name) =>
        CreateTaskCard(name);
}
