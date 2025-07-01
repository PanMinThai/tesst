using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;
using TodoList_Project.Features.Auth.Services.Interfaces;

namespace TodoList_Project.Features.Auth.Services
{
    public class LoginHistoryService : ILoginHistoryService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public LoginHistoryService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task RecordLoginAttemptAsync(Guid userId, bool isSuccess)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var history = new LoginHistoryEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                LoginTime = DateTime.UtcNow,
                IsSuccess = isSuccess,
            };

            await context.LoginHistories.AddAsync(history);
            await context.SaveChangesAsync();
        }
    }
}
