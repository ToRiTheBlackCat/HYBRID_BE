﻿using Hybrid.Repositories.Base;
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
        Task<(bool, string)> UpgradeTierOfUser(UpgradeTierRequest request);

    }
    public class TierService : ITierService
    {
        private StudentTierRepository _studentTierRepo => _unitOfWork.StudentTierRepo;
        private TeacherTierRepository _teacherTierRepo => _unitOfWork.TeacherTierRepo;

        private readonly UnitOfWork _unitOfWork;

        public TierService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// FUNC_GetAllTierOfStudent
        /// X
        /// List<TeacherTier>
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<List<StudentTier>> GetAllTierOfStudent()
        {
            var tierlist = await _studentTierRepo.GetAllAsync();
            return tierlist.ToList();
        }

        /// <summary>
        /// FUNC_GetAllTierOfTeacher
        /// X
        /// List<TeacherTier>
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<List<TeacherTier>> GetAllTierOfTeacher()
        {
            var tierlist = await _teacherTierRepo.GetAllAsync();
            return tierlist.ToList();
        }

        /// <summary>
        /// FUNC_GetTierOfStudentById
        /// tierId_String
        /// TierResponse?
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<TierResponse?> GetTierOfStudentById(string tierId)
        {
            var tier = await _studentTierRepo.GetByIdAsync(tierId);
            var tierResponse = tier?.Map_StudentTier_To_TierResponse();
            return tierResponse;
        }

        /// <summary>
        /// FUNC_GetTierOfTeacherById
        /// tierId_String
        /// TierResponse?
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<TierResponse?> GetTierOfTeacherById(string tierId)
        {
            var tier = await _teacherTierRepo.GetByIdAsync(tierId);
            var tierResponse = tier?.Map_TeacherTier_To_TierResponse();
            return tierResponse;
        }

        /// <summary>
        /// FUNC_UpgradeTierOfUser
        /// UpgradeTierRequest_ViewModel
        /// (bool, string)
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<(bool, string)> UpgradeTierOfUser(UpgradeTierRequest request)
        {
            if (request.IsTeacher)
            {
                var foundTeacher = await _unitOfWork.TeacherRepo.GetByIdAsync(request.UserId);
                if (foundTeacher == null)
                    return (false, "Cannot find teacher with that Id");

                foundTeacher.TierId = request.TierId;
                await _unitOfWork.TeacherRepo.UpdateAsync(foundTeacher);
                return (true, "Upgrade success");
            }
            else
            {
                var foundStudent = await _unitOfWork.StudentRepo.GetByIdAsync(request.UserId);
                if (foundStudent == null)
                    return (false, "Cannot find student with that Id");

                foundStudent.TierId = request.TierId;
                await _unitOfWork.StudentRepo.UpdateAsync(foundStudent);
                return (true, "Upgrade success");
            }
        }
    }
}
