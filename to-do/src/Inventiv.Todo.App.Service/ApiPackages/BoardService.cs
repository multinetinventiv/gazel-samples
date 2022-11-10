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
            codingStyle.AddTypes(t => t.ApiPackage("Board", s => s // Creates a virtual interface IBoardService and a virtual class BoardService, and registers them to service layer
                .Methods.Add(o => o.Proxy<IBoardManagerService>().TargetBySingleton(kernel)) // Include all methods of IBoardManagerService in virtual interface
                .Methods.Add(o => o.Proxy<IBoardsService>().TargetBySingleton(kernel)) // Include all methods of IBoardsService in virtual interface
                .Methods.Add(o => o.Proxy<IBoardService>().TargetByParameter<Board>()) // Include all methods of IBoardService in virtual interface
            ));
    }
}