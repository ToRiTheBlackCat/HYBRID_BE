using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
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
    }
}
