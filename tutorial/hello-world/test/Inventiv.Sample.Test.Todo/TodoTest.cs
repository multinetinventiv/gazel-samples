using Gazel;
using Gazel.UnitTesting;
using Inventiv.Sample.Module.Todo;

namespace Inventiv.Sample.Test.Todo;

[TestFixture]
public class TodoTest : TestBase
{
    static TodoTest()
    {
        Config.RootNamespace = "Inventiv";
    }

    [Test]
    public void SayHello__says_hello()
    {
        var todoManager = Context.Get<TodoManager>();

        Assert.AreEqual("Hello World!", todoManager.SayHello());
    }
}
