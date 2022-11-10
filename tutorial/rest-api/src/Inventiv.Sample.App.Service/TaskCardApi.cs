using Castle.MicroKernel;
using Gazel.Configuration;
using Inventiv.Sample.Module.Todo;
using Inventiv.Sample.Module.Todo.Service;
using Routine;
using Routine.Engine.Configuration.ConventionBased;

namespace Inventiv.Sample.App.Service;

public class TaskCardApi : ICodingStyleConfiguration
{
    public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel)
    {
        codingStyle.AddTypes(v => v.ApiPackage("TaskCard", t => t
            .Methods.Add(c => c.Proxy<ITaskCardService>().TargetByParameter<TaskCard>())
            .Methods.Add(c => c.Proxy<ITaskCardsService>().TargetBySingleton(kernel))
            .Methods.Add(c => c.Proxy<ITaskCardManagerService>().TargetBySingleton(kernel))
        ));
    }
}
