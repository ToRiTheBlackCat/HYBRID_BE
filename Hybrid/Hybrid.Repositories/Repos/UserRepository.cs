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

        public async Task<User?> GetUserByMailAsync(string email)
        {
            var user = await _dbSet
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x =>
                    x.Email.Equals(email) &&
                    x.IsActive
                );

            return user;
        }

        /// <summary>
        /// Automatically assign a UserId then create the new User
        /// </summary>
        public override void Create(User entity)
        {
            entity.UserId = GenerateId();
            base.Create(entity);
        }

        /// <summary>
        /// Automatically assign a UserId then create the new User asynchronously
        /// </summary>
        public override async Task<int> CreateAsync(User entity)
        {
            entity.UserId = GenerateId();
            return await base.CreateAsync(entity);
        }

        /// <summary>
        /// Generate UserId for new User
        /// </summary>
        private string GenerateId()
        {
            if (!_context.Users.Any())
            {
                return "HU0";
            }

            var newNumber = _context.Users
                .Select(u => int.Parse(u.UserId.Substring(2)))
                .ToList().Max() + 1;

            return "HU" + newNumber;
        }

        public async Task<(int, int)> GetUsersCountByRoleAsync()
        {
            var numberOfStudents = await _context.Users.CountAsync(c => c.RoleId == "2");
            var numberOfTeachers = await _context.Users.CountAsync(c => c.RoleId == "3");

            return (numberOfStudents, numberOfTeachers);
        }

        public async Task<(List<Student>, List<Teacher>)> GetUsersListByRoleAsync()
        {
            var studentList = await _context.Students.ToListAsync();
            var teacherList = await _context.Teachers.ToListAsync();

            return (studentList, teacherList);
        }
    }
}
