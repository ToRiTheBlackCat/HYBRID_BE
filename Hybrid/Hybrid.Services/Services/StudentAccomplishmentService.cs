using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Hybrid.Repositories.Repos;
using Hybrid.Services.Helpers;
using Hybrid.Services.ViewModel.Accomplishment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Services
{
    public interface IStudentAccomplishmentService
    {
        Task<AddAccomplishmentResponse> AddAccomplishmentAsync(AddAccomplishmentRequest request);
        Task<List<AccomplishmentViewModel>> GetAccomplishmentsAsync(string studentId, string minigameId);
    }

    public class StudentAccomplishmentService : IStudentAccomplishmentService
    {
        private readonly UnitOfWork _unitOfWork;
        public StudentAccomplishmentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Func - Add accomplishment for student
        /// Created By: TuanCA
        /// Created Date: 17/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<AddAccomplishmentResponse> AddAccomplishmentAsync(AddAccomplishmentRequest request)
        {
            var response = new AddAccomplishmentResponse
            {
                IsSuccess = false,
                Message = ""
            };

            // Check for user
            var student = await _unitOfWork.StudentRepo.GetByIdAsync(request.StudentId);
            if (student == null)
            {
                response.Message = $"Student not found with {nameof(request.StudentId)}.";
                return response;
            }

            // Check for minigame
            var minigame = await _unitOfWork.MiniGameRepo.GetByIdAsync(request.MinigameId);
            if (minigame == null)
            {
                response.Message = $"Minigame not found with {request.MinigameId}";
                return response;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = new StudentAccomplisment
                {
                    MinigameId = request.MinigameId,
                    Duration = request.DurationInSeconds,
                    Score = request.Percent,
                    TakenDate = request.TakenDate,
                    StudentId = student.UserId
                };
                _unitOfWork.StudentAccomplishmentRepo.Create(entity);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
                entity.Student = student; // Attach student to the entity for response
                response.Model = entity.ToViewModel();
            }
            catch (Exception ex)
            {
                response.Message = $"An error occurred while adding accomplishment: {ex.Message}";
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Accomplishment added successfully.";
            return response;
        }

        /// <summary>
        /// Func - Get all accomplishment of student in a minigame
        /// Created By: TuanCA
        /// Created Date: 17/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<List<AccomplishmentViewModel>> GetAccomplishmentsAsync(string studentId, string minigameId)
        {
            studentId = studentId?.Trim() ?? string.Empty;
            minigameId = minigameId.Trim();

            var accomplishments = await _unitOfWork.StudentAccomplishmentRepo
                .GetAllWithIncludeAsync(x => x.Student);
            if (accomplishments == null)
            {
                return new List<AccomplishmentViewModel>();
            }

            accomplishments = accomplishments.Where(x =>
                minigameId.Equals(x.MinigameId.Trim()) &&
                (string.IsNullOrEmpty(studentId) || x.StudentId.Trim().Equals(studentId))
            ).OrderBy(x => x.TakenDate);

            return accomplishments.Select(x => x.ToViewModel()).ToList();
        }
    }
}
