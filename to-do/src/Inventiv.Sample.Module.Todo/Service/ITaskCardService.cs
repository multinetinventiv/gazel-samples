using Inventiv.Sample.Module.Todo.Security;

namespace Inventiv.Sample.Module.Todo.Service;

public interface ITaskCardService
{
    // PATCH/PUT /task-cards/{id}
    // ContentType = application/json
    // {
    //  "name": string
    //  "notes": string
    //  "dueDate": string("YYYYMMDDHHmmss")
    // }
    void Update(string? name = default, string? notes = default, DateTime dueDate = default);

    // POST /task-cards/{id}/move
    // ContentType = application/json
    // {
    //  "columnId": int
    // }
    void Move(Column column);

    // POST /task-cards/{id}/assign
    // ContentType = application/json
    // {
    //  "userId": int
    // }
    void Assign(User user);

    // POST /task-cards/{id}/complete
    void Complete();

    // DELETE /task-cards/{id}
    void Delete();
}

public interface ITaskCardsService
{
    // GET /task-cards/{id}
    ITaskCardInfo GetTaskCard(int taskCardId);
}

public interface ITaskCardInfo
{
    int Id { get; }
    string Name { get; }
    DateTime DueDate { get; }
    string? Notes { get; }
    bool Completed { get; }
    User? User { get; }
}
