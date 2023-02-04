namespace Inventiv.Sample.Module.Todo.Mail;

public interface IMailClient // Abstraction for mail client so that you can mock it in unit tests
{
    void Send(string to, string subject, string content);
}
