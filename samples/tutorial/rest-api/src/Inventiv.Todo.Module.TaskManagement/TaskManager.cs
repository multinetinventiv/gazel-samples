using Gazel;
using Inventiv.Todo.Module.TaskManagement.Service;

namespace Inventiv.Todo.Module.TaskManagement
{
    public class TaskManager : ITaskManagerService
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

        #region Service Mappings

        ITaskInfo ITaskManagerService.CreateTask(string name) =>
            CreateTask(name);
        
        #endregion
    }
}