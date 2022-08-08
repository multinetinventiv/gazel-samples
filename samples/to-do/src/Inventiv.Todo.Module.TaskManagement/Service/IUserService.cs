using System;
using System.Collections.Generic;
using Gazel.DataAccess;

namespace Inventiv.Todo.Module.TaskManagement.Service
{
    public interface IUserManagerService
    {
        // POST /users
        // ContentType = application/json
        // {
        //  "name": string
        //  "email": string
        //  "password": string
        // }
        IUserInfo CreateUser(string name, Email email, Password password);
    }

    public interface IUserService
    {
        // GET /users/{id}/tasks
        List<ITaskInfo> GetTasks();

        // GET /users/{id}/boards
        List<IBoardInfo> GetBoards();
    }

    public interface IUsersService : IQuery
    {
        // GET /users?name={name}
        List<IUserInfo> GetUsers(string name);

        // GET /users/{id}
        IUserInfo GetUser(int userId);
    }

    public interface IUserInfo
    {
        int Id { get; }
        string Name { get; }
        Email Email { get; }
    }
}
