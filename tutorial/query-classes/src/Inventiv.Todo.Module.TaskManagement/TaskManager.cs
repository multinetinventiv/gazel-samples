using Gazel;

namespace Inventiv.Todo.Module.TaskManagement
{
    public class TaskManager
    {
        private readonly IModuleContext context;

        public TaskManager(IModuleContext context)
        {
            this.context = context;
        }

        public Task CreateTask(string name)
        {
            return context.New<Task>().With(name);
        }
    }
}