using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Repositories.Repos
{
    public class TransactionRepository : GenericRepository<TransactionHistory>
    {
        public TransactionRepository(HybridDBContext context) : base(context)
        {

        }

        /// <summary>
        /// Automatically assign a TransactionID then create the new TransactionHistory
        /// </summary>
        public override void Create(TransactionHistory entity)
        {
            entity.TransactionId = GenerateId();
            base.Create(entity);
        }

        /// <summary>
        /// Automatically assign a TransactionID then create the new TransactionHistory asynchronously
        /// </summary>
        public override async Task<int> CreateAsync(TransactionHistory entity)
        {
            entity.TransactionId = GenerateId();
            return await base.CreateAsync(entity);
        }

        /// <summary>
        /// Generate TransactionId for new User
        /// </summary>
        private string GenerateId()
        {
            if (!_context.TransactionHistories.Any())
            {
                return "Tra0";
            }

            var newNumber = _context.TransactionHistories
                .Select(u => int.Parse(u.TransactionId.Substring(3)))
                .ToList().Max() + 1;

            return "Tra" + newNumber;
        }
    }
}
