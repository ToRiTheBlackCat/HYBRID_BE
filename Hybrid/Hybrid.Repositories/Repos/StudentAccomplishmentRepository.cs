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
    public class StudentAccomplishmentRepository : GenericRepository<StudentAccomplisment>
    {
        public StudentAccomplishmentRepository(HybridDBContext context) : base(context)
        {
        }

        public async Task<List<StudentAccomplisment>> GetAccomplishmentsOfStudentAsync(string studentId)
        {
            return await _dbSet
                .Where(a => a.StudentId == studentId)
                .Include(a => a.Minigame).ThenInclude(m => m.Course)
                .Include(a => a.Minigame).ThenInclude(m => m.Template)
                .Include(a => a.Student)
                .ToListAsync();
        }
    }
}
