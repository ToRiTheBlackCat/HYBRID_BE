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
    public class TeacherSupscriptionRepository : GenericRepository<TeacherSupscription>
    {
        public TeacherSupscriptionRepository(HybridDBContext context) : base(context)
        {

        }
        public async Task<TeacherSupscription?> GetLatestSupscriptionOfTeacher(string teacherId)
        {
            var foundSupscription = await _context.TeacherSupscriptions
                .Where(x => x.TeacherId == teacherId && x.IsActive)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefaultAsync();

            return foundSupscription;
        }
    }
}
