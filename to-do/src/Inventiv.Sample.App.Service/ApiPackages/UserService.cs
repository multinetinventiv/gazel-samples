using Castle.MicroKernel;
using Gazel.Configuration;
using Inventiv.Sample.Module.Todo.Security;
using Inventiv.Sample.Module.Todo.Service;
using Routine.Engine.Configuration.ConventionBased;
using Routine;

namespace Inventiv.Sample.App.Service.ApiPackages;

public class UserService : ICodingStyleConfiguration
{
    public void Configure(ConventionBasedCodingStyle codingStyle, IKernel kernel) =>
        codingStyle.AddTypes(t => t.ApiPackage("User", s => s // Creates a virtual interface IUserService and and a virtual class UserService, and registers them to service layer
            .Methods.Add(c => c.Proxy<IUserManagerService>().TargetBySingleton(kernel)) // Include all methods of IUserManagerService in virtual interface
            .Methods.Add(c => c.Proxy<IUserService>().TargetByParameter<User>()) // Include all methods of IUserService in virtual interface
            .Methods.Add(c => c.Proxy<IUsersService>().TargetBySingleton(kernel)) // Include all methods of IUsersService in virtual interface
            )
        );
}
