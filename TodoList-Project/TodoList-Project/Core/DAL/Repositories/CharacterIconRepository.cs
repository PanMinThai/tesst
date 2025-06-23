using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;

namespace TodoList_Project.Core.DAL.Repositories
{
    public class CharacterIconRepository : ICharacterIconRepository
    {
        private readonly ApplicationDbContext _context;

        public CharacterIconRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CharacterIconEntity>> GetAllAsync()
        {
            return await _context.CharacterIcons.ToListAsync();
        }

        public async Task<CharacterIconEntity> GetByIdAsync(int id)
        {
            return await _context.CharacterIcons.FindAsync(id);
        }

        public async Task AddAsync(CharacterIconEntity entity)
        {
            _context.CharacterIcons.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CharacterIconEntity entity)
        {
            _context.CharacterIcons.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.CharacterIcons.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CharacterIconEntity>> GetByToneAsync(Tone tone)
        {
            return await _context.CharacterIcons
                .Where(i => i.Tone == tone)
                .ToListAsync();
        }
    }

}
