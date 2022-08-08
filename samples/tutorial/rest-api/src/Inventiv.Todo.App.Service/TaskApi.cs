using Castle.MicroKernel;
using Gazel.Configuration;
using Inventiv.Todo.Module.TaskManagement;
using Inventiv.Todo.Module.TaskManagement.Service;
using Routine;
using Routine.Engine.Configuration.ConventionBased;

namespace Inventiv.Todo.App.Service
{
    public class TaskApi : ICodingStyleConfiguration
    {
        public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel)
        {
            codingStyle.AddTypes(v => v.ApiPackage("Task", t => t
                .Methods.Add(c => c.Proxy<ITaskService>().TargetByParameter<Task>())
                .Methods.Add(c => c.Proxy<ITasksService>().TargetBySingleton(kernel))
                .Methods.Add(c => c.Proxy<ITaskManagerService>().TargetBySingleton(kernel))
            ));
        }
    }
}