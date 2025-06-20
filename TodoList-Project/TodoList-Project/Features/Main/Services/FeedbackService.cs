using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.DAL.Repositories;
using TodoList_Project.Core.Utils.Helpers;
using TodoList_Project.Features.Main.Models;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Main.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IMessageTemplateRepository _messageRepo;
        private readonly ICharacterIconRepository _iconRepo;
        private readonly IFeedbackRepository _feedbackRepo;

        public FeedbackService(
            ITaskRepository taskRepo,
            IMessageTemplateRepository messageRepo,
            ICharacterIconRepository iconRepo,
            IFeedbackRepository feedbackRepo)
        {
            _taskRepo = taskRepo;
            _messageRepo = messageRepo;
            _iconRepo = iconRepo;
            _feedbackRepo = feedbackRepo;
        }

        public async Task<FeedbackResponseModel> GetFeedbackForActionAsync(ActionType actionType)
        {
            var today = DateTime.UtcNow.Date;
            int actionCountToday = await GetActionCountAsync(actionType, today);

            var tone = GetToneByAction(actionType, actionCountToday);

            var usedMessages = await _feedbackRepo.GetUsedMessagesAsync(actionType.ToString(), tone, today);
            var usedIcons = await _feedbackRepo.GetUsedIconsAsync(actionType.ToString(), tone, today);

            var allMessages = await _messageRepo.GetByToneAsync(tone);
            var allIcons = await _iconRepo.GetByToneAsync(tone);

            var availableMessages = allMessages
                .Where(m => !usedMessages.Contains(m.Content))
                .ToList();

            var availableIcons = allIcons
                .Where(i => !usedIcons.Contains(i.ImagePath))
                .ToList();

            var finalMessage = (availableMessages.Any() ? availableMessages : allMessages).RandomItem()?.Content;
            var finalIcon = (availableIcons.Any() ? availableIcons : allIcons).RandomItem()?.ImagePath;

            var feedback = new UserFeedbackEntity
            {
                ActionType = actionType.ToString(),
                Message = finalMessage,
                ImagePath = finalIcon,
                Tone = tone,
                CreatedAt = DateTime.UtcNow
            };

            await _feedbackRepo.AddAsync(feedback);

            return new FeedbackResponseModel
            {
                Message = finalMessage,
                ImagePath = finalIcon,
                Tone = tone
            };
        }

        private async Task<int> GetActionCountAsync(ActionType actionType, DateTime today)
        {
            return actionType switch
            {
                ActionType.Cancelled => await _taskRepo.CountByStatusTodayAsync(TaskStatus.Cancelled),
                ActionType.Completed => await _taskRepo.CountByStatusTodayAsync(TaskStatus.Completed),
                // Can be extended for other action types in the future
                _ => 0
            };
        }

        /// <summary>
        /// Determines the appropriate tone based on the action type and count.
        /// </summary>
        /// <param name="actionType">The type of action being performed</param>
        /// <param name="count">The count associated with the action (used for certain action types)</param>
        /// <returns>The tone that should be used for the given action and count</returns>
        private Tone GetToneByAction(ActionType actionType, int count)
        {
            return actionType switch
            {
                // For cancelled actions in 1 day, tone varies based on count:
                // - 3 or fewer cancellations: sarcastic tone
                // - 4-6 cancellations: critical tone
                // - More than 6 cancellations: furious tone
                ActionType.Cancelled => count switch
                {
                    <= 3 => Tone.Sarcastic,
                    <= 6 => Tone.Critical,
                    _ => Tone.Furious
                },
                
                ActionType.Completed => count switch
                {
                    <= 3 => Tone.Praising,
                    <= 6 => Tone.Appreciative,
                    _ => Tone.Triumphant
                },
                ActionType.Create => count switch
                {
                    <= 3 => Tone.Encouraging,
                    <= 6 => Tone.Inspiring,
                    _ => Tone.Triumphant
                },
                ActionType.Update => count switch
                {
                    <= 3 => Tone.Formal,
                    _ => Tone.Cautious
                },
                ActionType.Delete => count switch
                {
                    <= 3 => Tone.Stern,
                    _ => Tone.Condemning
                },
                ActionType.Notification => count switch
                {
                    <= 3 => Tone.Neutral,
                    _ => Tone.Alert
                },
                ActionType.UndoCancelled => count switch
                {
                    <= 3 => Tone.Approving,
                    _ => Tone.Redeeming
                },
                ActionType.UndoCompleted => count switch
                {
                    <= 3 => Tone.Annoyed,
                    _ => Tone.Bitter
                },
            };
        }
    }


}
