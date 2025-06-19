using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Main.Models;

namespace TodoList_Project.Features.Main.Services
{
    public interface IFeedbackService
    {
        Task<FeedbackResponseModel> GetFeedbackForActionAsync(ActionType actionType);
    }


}
