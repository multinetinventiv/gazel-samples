using Castle.MicroKernel;
using Gazel;
using Gazel.Configuration;
using Inventiv.Sample.Module.Todo.Service;
using Routine;
using Routine.Engine.Configuration.ConventionBased;

namespace Inventiv.Sample.App.Service.ApiPackages;

public class AuthService : ICodingStyleConfiguration
{
    public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel) =>
        codingStyle.AddTypes(t => t.ApiPackage("Auth", s => s
            .Methods.Add(o => o.Proxy<IAuthManagerService>().TargetBySingleton(kernel))
        ))
        .OperationMarks.Add(Constants.MARK_FROM_FORM, m => m.Returns<ISessionInfo>("Login"));
}
