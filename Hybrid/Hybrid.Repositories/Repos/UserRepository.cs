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
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(HybridDBContext context) : base(context)
        {

        }

        public async Task<User?> GetUserAsync(string email, string password)
        {
            var user = await _dbSet.Include(x => x.Role)
                                   .FirstOrDefaultAsync(x => x.Email == email &&
                                                             x.Password == password &&
                                                             x.IsActive);
            return user;
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            var user = await _dbSet.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken &&
                                                             x.IsActive);
            return user;
        }
    }
}
