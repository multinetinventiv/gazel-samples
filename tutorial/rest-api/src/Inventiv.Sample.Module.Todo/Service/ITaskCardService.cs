namespace Inventiv.Sample.Module.Todo.Service;

public interface ITaskCardService
{
    void Complete();
}

public interface ITaskCardsService
{
    ITaskCardInfo GetTaskCard(int taskCardId);
    List<ITaskCardInfo> GetTaskCards(bool completed);
}

public interface ITaskCardManagerService
{
    ITaskCardInfo CreateTaskCard(string? name);
}

public interface ITaskCardInfo
{
    int Id { get; }
    string? Name { get; }
    bool Completed { get; }
}
