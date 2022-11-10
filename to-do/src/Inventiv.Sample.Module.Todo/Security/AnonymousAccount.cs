using Gazel.Security;

namespace Inventiv.Sample.Module.Todo.Security;

/// <summary>
/// Represents anonymous accounts
/// </summary>
public class AnonymousAccount : IAccount
{
    public int Id => -1;
    public string DisplayName => "Anonymous";

    public bool HasAccess(IResource resource) => true; //this gives access to everything. You may restrict anonymous users to specific services by implementing this method.
}
