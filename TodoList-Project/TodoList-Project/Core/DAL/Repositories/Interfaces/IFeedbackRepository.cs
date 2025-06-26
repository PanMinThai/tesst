using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;

namespace TodoList_Project.Core.DAL.Repositories.Interfaces
{
    public interface IFeedbackRepository : IGenericRepository<UserFeedbackEntity,int>
    {
        Task<List<string>> GetUsedMessagesAsync(string actionType, Tone tone, DateTime date);
        Task<List<string>> GetUsedIconsAsync(string actionType, Tone tone, DateTime date);
    }

}
