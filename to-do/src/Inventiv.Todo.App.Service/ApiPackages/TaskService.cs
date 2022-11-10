using Castle.MicroKernel;
using Gazel.Configuration;
using Inventiv.Todo.Module.TaskManagement;
using Inventiv.Todo.Module.TaskManagement.Service;
using Routine;
using Routine.Engine.Configuration.ConventionBased;

namespace Inventiv.Todo.App.Service.ApiPackages
{
    public class TaskService : ICodingStyleConfiguration
    {
        public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel) =>
            codingStyle.AddTypes(t => t.ApiPackage("Task", s => s // Creates a virtual interface ITaskService and and a virtual class TaskService, and registers them to service layer
                .Methods.Add(o => o.Proxy<ITasksService>().TargetBySingleton(kernel))  // Include all methods of ITasksService in virtual interface
                .Methods.Add(o => o.Proxy<ITaskService>().TargetByParameter<Task>())  // Include all methods of ITaskService in virtual interface
            ));
    }
}
