using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Repositories.Repos
{
    public class MinigameTemplateRepository : GenericRepository<MinigameTemplate>
    {
        public MinigameTemplateRepository(HybridDBContext context) : base(context)
        {
        }
    }
}
