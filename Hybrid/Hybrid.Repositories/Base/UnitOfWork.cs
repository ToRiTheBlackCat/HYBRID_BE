using Hybrid.Repositories.Models;
using Hybrid.Repositories.Repos;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Repositories.Base
{
    public class UnitOfWork
    {
        private readonly HybridDBContext _context;
        private IDbContextTransaction? _transaction;

        public UserRepository UserRepo { get; }
        public StudentRepository StudentRepo { get; }
        public TeacherRepository TeacherRepo { get; }
        public TransactionRepository TransactionRepo { get; }
        public SupscriptionExtentionRepository SupscriptionExtentionRepo { get; }
        public StudentSupscriptionRepository StudentSupscriptionRepo { get; }

        public TeacherSupscriptionRepository TeacherSupscriptionRepo { get; }

        public UnitOfWork(HybridDBContext context,
                          UserRepository userRepo,
                          StudentRepository studentRepo,
                          TeacherRepository teacherRepo,
                          TransactionRepository transactionRepo,
                          StudentSupscriptionRepository studentSupscriptionRepo,
                          TeacherSupscriptionRepository teacherSupscriptionRepo,
                          SupscriptionExtentionRepository supscriptionExtentionRepo)
        {
            _context = context;
            UserRepo = userRepo;
            StudentRepo = studentRepo;
            TeacherRepo = teacherRepo;
            TransactionRepo = transactionRepo;
            StudentSupscriptionRepo = studentSupscriptionRepo;
            TeacherSupscriptionRepo = teacherSupscriptionRepo;
            SupscriptionExtentionRepo = supscriptionExtentionRepo;
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
