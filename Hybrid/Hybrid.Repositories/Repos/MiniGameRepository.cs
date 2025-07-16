using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
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

        public async Task<List<Minigame>> GetMinigamesOfTeacherAsync(string teacherId, string templateId, string minigameName)
        {
            var list = await _context.Minigames
                .Include(x => x.Template)
                .Include(x => x.Teacher)
                .Include(x => x.Ratings)
                .Include(x => x.StudentAccomplisments)
                .Where(x =>
                    x.TeacherId.Trim().Equals(teacherId) &&
                    (string.IsNullOrEmpty(templateId) || x.TemplateId.Trim().Equals(templateId)) &&
                    (string.IsNullOrEmpty(minigameName) || x.MinigameName.Trim().Contains(minigameName))
                )
                .OrderBy(x => x.MinigameName)
                .ToListAsync();

            return list;
        }

        public async Task<List<Minigame>> GetMinigamesOfCourseAsync(string courseId, string templateId, string minigameName)
        {
            var list = await _context.Minigames
                .Include(x => x.Template)
                .Include(x => x.Teacher)
                .Include(x => x.Ratings)
                .Include(x => x.StudentAccomplisments)
                .Where(x =>
                    x.CourseId.Trim().Equals(courseId) &&
                    (string.IsNullOrEmpty(templateId) || x.TemplateId.Trim().Equals(templateId)) &&
                    (string.IsNullOrEmpty(minigameName) || x.MinigameName.Trim().Contains(minigameName))
                )
                .OrderBy(x => x.MinigameName)
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

        public async Task<Dictionary<string, int>> GetMinigamesCountByTemplateNameAsync()
        {
            var result = await _context.MinigameTemplates
                .GroupJoin(
                    _context.Minigames,
                    template => template.TemplateId,
                    game => game.TemplateId,
                    (template, games) => new
                    {
                        TemplateName = template.TemplateName,
                        Count = games.Count()
                    })
                .OrderBy(x => x.TemplateName)
                .ToListAsync();

            return result.ToDictionary(x => x.TemplateName, x => x.Count);
        }
    }
}
