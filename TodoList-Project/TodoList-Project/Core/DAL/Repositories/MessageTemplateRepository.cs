using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Add this using directive
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;

namespace TodoList_Project.Core.DAL.Repositories
{
    public class MessageTemplateRepository : IMessageTemplateRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageTemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MessageTemplateEntity>> GetAllAsync()
        {
            return await _context.MessageTemplates.ToListAsync();
        }

        public async Task<MessageTemplateEntity> GetByIdAsync(object id)
        {
            return await _context.MessageTemplates.FindAsync(id);
        }

        public async Task AddAsync(MessageTemplateEntity entity)
        {
            _context.MessageTemplates.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MessageTemplateEntity entity)
        {
            _context.MessageTemplates.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.MessageTemplates.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<MessageTemplateEntity>> GetByToneAsync(Tone tone)
        {
            return await _context.MessageTemplates
                .Where(m => m.Tone == tone)
                .ToListAsync();
        }
    }

}
