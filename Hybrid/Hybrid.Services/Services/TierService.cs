using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Hybrid.Repositories.Repos;
using Hybrid.Services.Helpers;
using Hybrid.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Services
{
    public interface ITierService
    {
        Task<List<StudentTier>> GetAllTierOfStudent();
        Task<List<TeacherTier>> GetAllTierOfTeacher();
        Task<TierResponse?> GetTierOfStudentById(string tierId);
        Task<TierResponse?> GetTierOfTeacherById(string tierId);


    }
    public class TierService : ITierService
    {
        private  StudentTierRepository _studentTierRepo => _unitOfWork.StudentTierRepo;
        private  TeacherTierRepository _teacherTierRepo => _unitOfWork.TeacherTierRepo;

        private readonly UnitOfWork _unitOfWork;

        public TierService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<StudentTier>> GetAllTierOfStudent()
        {
            var tierlist = await _studentTierRepo.GetAllAsync();
            return tierlist.ToList();
        }

        public async Task<List<TeacherTier>> GetAllTierOfTeacher()
        {
            var tierlist = await _teacherTierRepo.GetAllAsync();
            return tierlist.ToList();
        }

        public async Task<TierResponse?> GetTierOfStudentById(string tierId)
        {
            var tier = await _studentTierRepo.GetByIdAsync(tierId);
            var tierResponse = tier?.Map_StudentTier_To_TierResponse();
            return tierResponse;
        }

        public async Task<TierResponse?> GetTierOfTeacherById(string tierId)
        {
            var tier = await _teacherTierRepo.GetByIdAsync(tierId);
            var tierResponse = tier?.Map_TeacherTier_To_TierResponse();
            return tierResponse;
        }
    }
}
