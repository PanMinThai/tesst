using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Auth.Services.Interfaces
{
    public interface ILoginHistoryService
    {
        Task RecordLoginAttemptAsync(Guid userId, bool isSuccess);
    }
}
