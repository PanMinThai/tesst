using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.DAL.Repositories.Interfaces;

namespace TodoList_Project.Core.DAL.Repositories.General
{
    public class CharacterIconRepository : GenericRepository<CharacterIconEntity, int>, ICharacterIconRepository
    {
        public CharacterIconRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<List<CharacterIconEntity>> GetByToneAsync(Tone tone)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<CharacterIconEntity>()
                .Where(i => i.Tone == tone)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}