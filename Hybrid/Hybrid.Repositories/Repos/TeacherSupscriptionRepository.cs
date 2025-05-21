using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
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
    }
}
