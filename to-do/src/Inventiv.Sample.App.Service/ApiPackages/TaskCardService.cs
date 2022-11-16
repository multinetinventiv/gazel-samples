using Castle.MicroKernel;
using Gazel.Configuration;
using Inventiv.Sample.Module.Todo;
using Inventiv.Sample.Module.Todo.Service;
using Routine;
using Routine.Engine.Configuration.ConventionBased;

namespace Inventiv.Sample.App.Service.ApiPackages;

public class TaskCardService : ICodingStyleConfiguration
{
    public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel) =>
        codingStyle.AddTypes(t => t.ApiPackage("TaskCard", s => s // Creates a virtual interface ITaskCardService and and a virtual class TaskCardService, and registers them to service layer
            .Methods.Add(o => o.Proxy<ITaskCardsService>().TargetBySingleton(kernel))  // Include all methods of ITaskCardsService in virtual interface
            .Methods.Add(o => o.Proxy<ITaskCardService>().TargetByParameter<TaskCard>())  // Include all methods of ITaskCardService in virtual interface
        ));
}
