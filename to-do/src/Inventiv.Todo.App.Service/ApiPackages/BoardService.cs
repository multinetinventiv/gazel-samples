using Castle.MicroKernel;
using Gazel.Configuration;
using Inventiv.Todo.Module.TaskManagement;
using Inventiv.Todo.Module.TaskManagement.Service;
using Routine;
using Routine.Engine.Configuration.ConventionBased;

namespace Inventiv.Todo.App.Service
{
    public class BoardService : ICodingStyleConfiguration
    {
        public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel) =>
            codingStyle.AddTypes(t => t.ApiPackage("Board", s => s
            .Methods.Add(o => o.Proxy<IBoardManagerService>().TargetBySingleton(kernel))
            .Methods.Add(o => o.Proxy<IBoardsService>().TargetBySingleton(kernel))
            .Methods.Add(o => o.Proxy<IBoardService>().TargetByParameter<Board>())
            ));
    }
}