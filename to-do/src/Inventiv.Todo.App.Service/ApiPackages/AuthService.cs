using Gazel;
using Routine;
using Castle.MicroKernel;
using Gazel.Configuration;
using Inventiv.Todo.Module.TaskManagement.Security;
using Inventiv.Todo.Module.TaskManagement.Service;
using Routine.Engine.Configuration.ConventionBased;

namespace Inventiv.Todo.App.Service.ApiPackages
{
    public class AuthService : ICodingStyleConfiguration
    {
        public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel) =>
            codingStyle.AddTypes(t => t.ApiPackage("Auth", s => s
            .Methods.Add(o => o.Proxy<IAuthManagerService>().TargetBySingleton(kernel))
            )).OperationMarks.Add(Constants.MARK_FROM_FORM, m => m.Returns<ISessionInfo>("Login"));
    }
}
