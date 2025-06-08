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
    public class RatingRepository : GenericRepository<Rating>
    {
        public RatingRepository(HybridDBContext context) : base(context)
        {
        }

        public async Task<List<Rating>?> GetAllByMinigameId(string minigameId)
        {
            var ratingList = await _context.Ratings
                .Where(x => x.MinigameId.Equals(minigameId))
                .Include(x => x.Student)
                .ToListAsync();

            return ratingList;
        }
        
        public async Task<Rating?> GetExistedRating(string minigameId, string studentId)
        {
            var foundRating = await _context.Ratings
                .SingleOrDefaultAsync(x => x.MinigameId.Equals(minigameId) && x.StudentId.Equals(studentId));   

            return foundRating;
        }

    }
}
