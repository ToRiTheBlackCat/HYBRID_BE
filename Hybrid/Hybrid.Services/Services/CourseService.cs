using Azure.Core;
using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Hybrid.Services.Helpers;
using Hybrid.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Services
{
    public interface ICourseService
    {
        Task<List<CoursesResponse>?> GetAllCourses(string? courseName, string? levelId, int currentPage, int pageSize);
        Task<DetailCoursesResponse?> GetCourseById(string? courseId);
        Task<(bool, string)> CreateCourse(CreateCourseRequest request);
        Task<(bool, string)> UpdateCourse(UpdateCourseRequest request);
    }
    public class CourseService : ICourseService
    {
        private readonly UnitOfWork _unitOfWork;
        public CourseService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool, string)> CreateCourse(CreateCourseRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var newCourse = new Course
                {
                    CourseName = request.CourseName,
                    DataText = request.DataText,
                    LevelId = request.LevelId,
                };

                newCourse.CourseId = _unitOfWork.CourseRepo.GenerateId(newCourse);

                await _unitOfWork.CourseRepo.CreateAsync(newCourse);
                await _unitOfWork.CommitTransactionAsync();

                return (true, "Create successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine(ex.Message);
                return (false, "Create fail");
            }
        }

        public async Task<List<CoursesResponse>?> GetAllCourses(string? courseName, string? levelId, int currentPage, int pageSize)
        {
            var listCourses = new List<Course>();

            if (string.IsNullOrWhiteSpace(courseName) && string.IsNullOrEmpty(levelId))
            {
                listCourses = await _unitOfWork.CourseRepo.GetALlCourseAsync();
            }
            else
            {
                listCourses = await _unitOfWork.CourseRepo.SearchCourese(courseName, levelId);
            }

            var pagedList = listCourses.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var result = pagedList.Map_ListCourse_To_ListCoursesResponse();

            return result;
        }

        public async Task<DetailCoursesResponse?> GetCourseById(string? courseId)
        {
            var result = new DetailCoursesResponse();

            var foundCourse = await _unitOfWork.CourseRepo.GetCourseByIdAsync(courseId);
            if (foundCourse != null)
            {
                result = foundCourse.Map_Course_To_DetailCoursesResponse();
            }
            return result;
        }

        public async Task<(bool, string)> UpdateCourse(UpdateCourseRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var mappedCourse = request.Map_UpdateCourseRequest_To_Course();

                var result = await _unitOfWork.CourseRepo.UpdateAsync(mappedCourse);
                await _unitOfWork.CommitTransactionAsync();
                return (true, "Update successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine(ex.Message);
                return (false, "Update fail");
            }
        }
    }
}

