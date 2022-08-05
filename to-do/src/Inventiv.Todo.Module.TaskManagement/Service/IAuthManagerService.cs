using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventiv.Todo.Module.TaskManagement.Service
{
    public interface IAuthManagerService
    {
        // POST /users/login
        // Content-Type=x-www-form-urlencoded
        // email={email}&password={password}
        ISessionInfo Login(Email email, Password password);
    }

    public interface ISessionInfo
    {
        AppToken Token { get; }
        IUserInfo User { get; }
        DateTime ExpireDateTime { get; }
    }
}
