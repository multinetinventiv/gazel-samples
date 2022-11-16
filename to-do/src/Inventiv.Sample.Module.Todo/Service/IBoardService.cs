using Gazel.DataAccess;
using Inventiv.Sample.Module.Todo.Security;

namespace Inventiv.Sample.Module.Todo.Service;

public interface IBoardManagerService
{
    // POST /boards
    // ContentType = application/json
    // {
    //  "name": string
    // }
    IBoardInfo CreateBoard(string name);
}

public interface IBoardService
{
    // PATCH /boards/{id}
    // ContentType = application/json
    // {
    //  "name": string
    // }
    void Update(string name);

    // POST /boards/{id}/columns
    // ContentType = application/json
    // {
    //  "name": string
    // }
    IColumnInfo AddColumn(string name);

    // GET /boards/{id}/columns
    List<IColumnInfo> GetColumns();

    // POST /boards/{id}/users
    // ContentType = application/json
    // {
    //  "userId": int
    // }
    void AddUser(User user);

    // GET /boards/{id}/users
    List<IUserInfo> GetUsers();

    // DELETE /boards/{id}/users/{child-id}
    void RemoveUser(User user);

    // DELETE /boards/{id}
    void Delete();
}

public interface IBoardsService : IQuery
{
    // GET /boards?name={name}
    List<IBoardInfo> GetBoards(string name);

    // GET /boards/{id}
    IBoardDetail GetBoard(int boardId);
}

public interface IBoardInfo
{
    int Id { get; }
    string Name { get; }
}

public interface IBoardDetail
{
    int Id { get; }
    string Name { get; }
    List<IColumnDetail> Columns { get; } // Eager fetching in web service layer
    List<IUserInfo> Users { get; }  // Eager fetching in web service layer
}
