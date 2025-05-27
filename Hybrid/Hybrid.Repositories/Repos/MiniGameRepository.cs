using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Repositories.Repos
{
    public class MiniGameRepository : GenericRepository<Minigame>
    {
        public MiniGameRepository(HybridDBContext context) : base(context)
        {
        }

        public async Task<List<Minigame>> GetMinigamesOfTeacherAsync(string teacherId)
        {
            var list = await _context.Minigames
                .Include(x => x.Template)
                .Where(x => x.TeacherId.Trim().Equals(teacherId))
                .ToListAsync();

            return list;
        }

        /// <summary>
        /// Generate MiniGameId for new MiniGame
        /// </summary>
        public string GenerateId(Minigame minigame)
        {
            if (!_context.Minigames.Any())
            {
                return "MG0";
            }

            var newNumber = _context.Minigames
                .Select(u => int.Parse(u.MinigameId.Substring(2)))
                .ToList().Max() + 1;

            return "MG" + newNumber;
        }
    }
}
