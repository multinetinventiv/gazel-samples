using System.Collections.Generic;

namespace Inventiv.Todo.Module.TaskManagement.Service
{
    public interface ITaskService
    {
        void Complete();
    }

    public interface ITasksService
    {
        ITaskInfo GetTask(int taskId);
        List<ITaskInfo> GetTasks(bool completed);
    }

    public interface ITaskManagerService
    {
        ITaskInfo CreateTask(string name);
    }

    public interface ITaskInfo
    {
        int Id { get; }
        string Name { get; }
        bool Completed { get; }
    }
}