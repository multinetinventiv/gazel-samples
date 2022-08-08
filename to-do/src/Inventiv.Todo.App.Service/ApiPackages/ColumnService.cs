using Castle.MicroKernel;
using Gazel.Configuration;
using Inventiv.Todo.Module.TaskManagement;
using Inventiv.Todo.Module.TaskManagement.Service;
using Routine;
using Routine.Engine.Configuration.ConventionBased;

namespace Inventiv.Todo.App.Service.ApiPackages
{
    public class ColumnService : ICodingStyleConfiguration
    {
        public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel) =>
            codingStyle.AddTypes(t => t.ApiPackage("Column", s => s // Creates a virtual interface IColumnService and and a virtual class ColumnService, and registers them to service layer
                .Methods.Add(o => o.Proxy<IColumnService>().TargetByParameter<Column>()) // Include all methods of IColumnService in virtual interface
                .Methods.Add(o => o.Proxy<IColumnsService>().TargetBySingleton(kernel)) // Include all methods of IColumnsService in virtual interface
            ));
    }
}
