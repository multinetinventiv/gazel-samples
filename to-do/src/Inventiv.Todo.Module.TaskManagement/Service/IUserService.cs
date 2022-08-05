using System;
using System.Collections.Generic;
using Gazel.DataAccess;

namespace Inventiv.Todo.Module.TaskManagement.Service
{
    public interface IUserManagerService
    {
        // POST /users
        // Content-Type=application/json
        // {
        //		"name": string
        //		"email": string
        //		"password": string
        // }
        IUserInfo CreateUser(string name, Email email, Password password);
    }

    public interface IUserService
    {
        // GET /users/{user_id}/tasks
        List<ITaskInfo> GetTasks();

        // GET /users/{user_id}/boards
        List<IBoardInfo> GetBoards();
    }

    public interface IUsersService : IQuery
    {
        // GET /users?name={name}
        List<IUserInfo> GetUsers(string name);

        // GET /users/{user_id}
        IUserInfo GetUser(int userId);
    }

    public interface IUserInfo
    {
        int Id { get; }
        string Name { get; }
        Email Email { get; }
    }
}
