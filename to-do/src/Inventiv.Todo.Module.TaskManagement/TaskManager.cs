using Gazel;
using Inventiv.Todo.Module.TaskManagement.Service;

namespace Inventiv.Todo.Module.TaskManagement
{
    public class TaskManager : IBoardManagerService
    {
        #region IoC

        private readonly IModuleContext context;

        public TaskManager(IModuleContext context)
        {
            this.context = context;
        }

        #endregion

        public Board CreateBoard(string name) => context.New<Board>().With(name);

        #region Web Service Mappings

        IBoardInfo IBoardManagerService.CreateBoard(string name) => CreateBoard(name);

        #endregion
    }
}
