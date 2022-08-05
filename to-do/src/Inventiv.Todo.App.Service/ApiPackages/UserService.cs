using Gazel;
using Routine;
using Castle.MicroKernel;
using Gazel.Configuration;
using Inventiv.Todo.Module.TaskManagement.Security;
using Inventiv.Todo.Module.TaskManagement.Service;
using Routine.Engine.Configuration.ConventionBased;

namespace Inventiv.Todo.App.Service.ApiPackages
{
    public class UserService : ICodingStyleConfiguration
    {
        public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel) =>
            codingStyle.AddTypes(t => t.ApiPackage("User", s => s
            .Methods.Add(o => o.Proxy<IUsersService>().TargetBySingleton(kernel))
            )).OperationMarks.Add(Constants.MARK_FROM_FORM, m => m.Returns<ISessionInfo>("Login"));
    }
}
