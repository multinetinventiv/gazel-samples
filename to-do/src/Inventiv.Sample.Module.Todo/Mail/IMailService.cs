namespace Inventiv.Sample.Module.Todo.Mail;

public interface IMailService // Abstraction for mail service so that you can mock it in unit testss
{
    void Send(string to, string subject, string content);
}
