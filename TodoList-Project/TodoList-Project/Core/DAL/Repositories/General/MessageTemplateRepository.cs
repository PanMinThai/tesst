using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Add this using directive
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.DAL.Repositories.Interfaces;

namespace TodoList_Project.Core.DAL.Repositories.General
{
    public class MessageTemplateRepository : GenericRepository<MessageTemplateEntity,int>,IMessageTemplateRepository
    {
        public MessageTemplateRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory)
        {
        }
        public async Task<List<MessageTemplateEntity>> GetByToneAsync(Tone tone)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<MessageTemplateEntity>()
                .Where(m => m.Tone == tone)
                .AsNoTracking()  
                .ToListAsync();
        }
    }

}
