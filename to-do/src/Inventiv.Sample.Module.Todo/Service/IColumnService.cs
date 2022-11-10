using Gazel.DataAccess;

namespace Inventiv.Sample.Module.Todo.Service;

public interface IColumnService
{
    // PATCH /columns/{id}
    // ContentType = applications/json
    // {
    //  "name": string
    // }
    void Update(string name);

    // POST /columns/{id}/task-cards
    // ContentType = applications/json
    // {
    //  "name": string
    // }
    ITaskCardInfo AddTaskCard(string name);

    // GET /columns/{id}/task-cards
    List<ITaskCardInfo> GetTaskCards();

    // POST /columns/{id}/clear
    void Clear();

    // DELETE /columns/{id}
    void Delete();

    // POST /columns/{id}/move-task-cards-and-delete
    // ContentType = application/json
    // {
    //  "targetColumnId": int
    // }
    void MoveTaskCardsAndDelete(Column targetColumn);
}

public interface IColumnsService : IQuery
{
    // GET /columns/{id}
    IColumnDetail GetColumn(int columnId);
}

public interface IColumnInfo
{
    int Id { get; }
    string Name { get; }
    Board Board { get; }
}

public interface IColumnDetail
{
    int Id { get; }
    string Name { get; }
    List<ITaskCardInfo> TaskCards { get; } // Eager fetching in rest api layer
}
