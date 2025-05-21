using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Repositories.Repos
{
    public class TeacherTierRepository : GenericRepository<TeacherTier>
    {
        public TeacherTierRepository(HybridDBContext context) : base(context)
        {

        }
    }
}
