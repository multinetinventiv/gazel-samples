using System;
using Inventiv.Todo.Module.TaskManagement.Security;

namespace Inventiv.Todo.Module.TaskManagement.Service
{
    public interface ITaskService
    {
        // PATCH/PUT /tasks/{id}
        // ContentType = application/json
        // {
        //  "name": string
        //  "notes": string
        //  "dueDate": string("YYYYMMDDHHmmss")
        // }
        void Update(string name, string notes);
        void Update(DateTime dueDate);
        void Update(string name, string notes, DateTime dueDate);

        // POST /tasks/{id}/move
        // ContentType = application/json
        // {
        //  "columnId": int
        // }
        void Move(Column column);

        // POST /tasks/{id}/assign
        // ContentType = application/json
        // {
        //  "userId": int
        // }
        void Assign(User user);

        // POST /tasks/{id}/complete
        void Complete();

        // DELETE /tasks/{id}
        void Delete();
    }

    public interface ITasksService
    {
        // GET /tasks/{id}
        ITaskInfo GetTask(int taskId);
    }

    public interface ITaskInfo
    {
        int Id { get; }
        string Name { get; }
        DateTime DueDate { get; }
        string Notes { get; }
        bool Completed { get; }
        User User { get; }
    }
}
