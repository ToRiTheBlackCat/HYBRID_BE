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
    public class StudentSupscriptionRepository : GenericRepository<StudentSupscription>
    {
        public StudentSupscriptionRepository(HybridDBContext context) : base(context)
        {

        }

        public async Task<StudentSupscription?> GetLatestSupscriptionOfStudent(string studentId)
        {
            var foundScription = await _context.StudentSupscriptions
                .Where(x => x.StudentId == studentId && x.IsActive)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefaultAsync();

            return foundScription;
        }
    }
}
