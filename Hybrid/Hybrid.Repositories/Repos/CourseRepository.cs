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

        public async Task<(int,int,int)> GetALlCourseByLevelAsync()
        {
            List<string> listLevelId = new List<string>()
            {
                "1","2","3"
            };
            var startersCourseCount = await _context.Courses.CountAsync(c => c.LevelId == listLevelId[0]);
            var moversCourseCount = await _context.Courses.CountAsync(c => c.LevelId == listLevelId[1]);
            var flyersCourseCount = await _context.Courses.CountAsync(c => c.LevelId == listLevelId[2]);

            return (startersCourseCount,moversCourseCount,flyersCourseCount);
        }
    }
}
