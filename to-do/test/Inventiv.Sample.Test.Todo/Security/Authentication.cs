using Gazel.Security;

namespace Inventiv.Sample.Test.Todo.Security;

[TestFixture]
public class Authentication : ToDoTestBase
{
    [Test]
    public void GIVEN_in_any_condition__WHEN_user_creates_a_user_with_name__email_and_password__THEN_the_actual__s_name_and_email_should_equal_to_given_name_and_email()
    {
        BeginTest();

        var user = _securityManager.CreateUser("test", Email.Parse("test@gazel.io"), APassword());

        Verify.ObjectIsPersisted(user);
        Assert.That(user.Name, Is.EqualTo("test"));
        Assert.That(user.Email.Value, Is.EqualTo("test@gazel.io"));
    }

    [Test]
    public void GIVEN_there_exists_a_user__WHEN_the_user_logs_in__THEN_a_session_is_returned_with_a_new_token_that_expires_in_one_day()
    {
        CreateUser(
            email: "test@gazel.io",
            password: "1234"
        );

        SetUpAppTokens(AppToken.Parse("test_token"));
        SetUpTime(DT("20180211", "173801"));

        BeginTest();

        var session = _securityManager.Login(Email.Parse("test@gazel.io"), Password.Parse("1234"));

        Assert.That(session.Token.ToString(), Is.EqualTo("test_token"));
        Assert.That(session.ExpireDateTime, Is.EqualTo(DT("20180212", "173801")));
    }

    [Test]
    public void GIVEN_there_exists_an_expired_token__WHEN_user_uses_this_token__THEN_system_requires_a_new_login_for_user_to_proceed()
    {
        SetUpSession(
            CreateUserSession(
                expired: true
            )
        );

        BeginTest();

        Assert.That(() => Context.Session.Validate(), Throws.TypeOf<AuthenticationRequiredException>());
    }
}
