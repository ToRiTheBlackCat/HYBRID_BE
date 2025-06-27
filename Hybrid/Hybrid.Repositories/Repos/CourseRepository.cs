using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Repositories.Repos
{
    public class CourseRepository : GenericRepository<Course>
    {
        public CourseRepository(HybridDBContext context) : base(context)
        {
        }
        public async Task<List<Course>> GetALlCourseAsync()
        {
            var list = await _context.Courses
                .Include(x => x.Level)
                .OrderByDescending(x => x.CourseId)
                .ToListAsync();

            return list;
        }

        public async Task<Course?> GetCourseByIdAsync(string? courseId)
        {
            var foundCourse = await _context.Courses
                .Include(x => x.Level)
                .FirstOrDefaultAsync(x => x.CourseId.Equals(courseId));

            return foundCourse;
        }

        public async Task<List<Course>> SearchCourese(string? courseName, string? levelId)
        {
            var list = await _context.Courses
                .Include(x => x.Level)
                .Where(x => x.CourseName.Contains(courseName) || x.LevelId.Contains(levelId))
                .OrderByDescending(x => x.CourseId)
                .ToListAsync();

            return list;
        }

        public string GenerateId(Course course)
        {
            if (!_context.Courses.Any())
            {
                return "CO0";
            }

            var newNumber = _context.Courses
                .Select(u => int.Parse(u.CourseId.Substring(2)))
                .ToList().Max() + 1;

            return "CO" + newNumber;
        }

        public async Task<(int Starters, int Movers, int Flyers)> GetAllCourseByLevelAsync()
        {
            var levelCounts = await _context.Courses
                .Where(c => c.LevelId == "1" || c.LevelId == "2" || c.LevelId == "3")
                .GroupBy(c => c.LevelId)
                .Select(g => new { LevelId = g.Key, Count = g.Count() })
                .ToListAsync();

            int starters = levelCounts.FirstOrDefault(g => g.LevelId == "1")?.Count ?? 0;
            int movers = levelCounts.FirstOrDefault(g => g.LevelId == "2")?.Count ?? 0;
            int flyers = levelCounts.FirstOrDefault(g => g.LevelId == "3")?.Count ?? 0;

            return (starters, movers, flyers);
        }

    }
}
