using Azure.Core;
using Hybrid.Repositories.Models;
using Hybrid.Services.ViewModel.Accomplishment;
using Hybrid.Services.ViewModel.Course;
using Hybrid.Services.ViewModel.Minigames;
using Hybrid.Services.ViewModel.Profile;
using Hybrid.Services.ViewModel.Rating;
using Hybrid.Services.ViewModel.SignUp;
using Hybrid.Services.ViewModel.SupscriptionExtention;
using Hybrid.Services.ViewModel.Tier;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Helpers
{
    public static class Mapper
    {
        public static User Map_SignUpUserVM_To_User(this SignUpUserRequest signUpRequestViewModel)
        {
            return new User
            {
                Email = signUpRequestViewModel.Email,
                Password = signUpRequestViewModel.Password,
                CreatedDate = DateTime.Now,
                RoleId = signUpRequestViewModel.RoleId,
                IsActive = true
            };
        }

        public static Student Map_SignUpTeacher_StudentRequestVM_To_Student(this SignUpTeacher_StudentRequest signUpStudentRequestViewModel)
        {
            return new Student
            {
                UserId = signUpStudentRequestViewModel.UserId,
                FullName = signUpStudentRequestViewModel.FullName,
                Address = signUpStudentRequestViewModel.Address,
                Phone = signUpStudentRequestViewModel.Phone,
                YearOfBirth = signUpStudentRequestViewModel.YearOfBirth,
                TierId = "1"
            };
        }

        public static Teacher Map_SignUpTeacher_StudentRequestVM_To_Teacher(this SignUpTeacher_StudentRequest signUpTeacherRequestViewModel)
        {
            return new Teacher
            {
                UserId = signUpTeacherRequestViewModel.UserId,
                FullName = signUpTeacherRequestViewModel.FullName,
                Address = signUpTeacherRequestViewModel.Address,
                Phone = signUpTeacherRequestViewModel.Phone,
                YearOfBirth = signUpTeacherRequestViewModel.YearOfBirth,
                TierId = "1"
            };
        }

        public static StudentSupscription Map_SupscriptionExtentionOrder_To_StudentSupscription(this SupscriptionExtentionOrder supscriptionExtentionOrder)
        {
            return new StudentSupscription
            {
                StudentId = supscriptionExtentionOrder.UserId,
                TierId = supscriptionExtentionOrder.TierId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(supscriptionExtentionOrder.Days),
                IsActive = true
            };
        }

        public static TeacherSupscription Map_SupscriptionExtentionOrder_To_TeacherSupscription(this SupscriptionExtentionOrder supscriptionExtentionOrder)
        {
            return new TeacherSupscription
            {
                TeacherId = supscriptionExtentionOrder.UserId,
                TierId = supscriptionExtentionOrder.TierId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(supscriptionExtentionOrder.Days),
                IsActive = true
            };
        }

        public static TierResponse Map_StudentTier_To_TierResponse(this StudentTier studentTier)
        {
            return new TierResponse
            {
                TierId = studentTier.TierId.Trim(),
                TierName = studentTier.TierName.Trim(),
                Description = studentTier.Description.Trim()
            };
        }

        public static TierResponse Map_TeacherTier_To_TierResponse(this TeacherTier teacherTier)
        {
            return new TierResponse
            {
                TierId = teacherTier.TierId.Trim(),
                TierName = teacherTier.TierName.Trim(),
                Description = teacherTier.Description.Trim()
            };
        }

        public static SupscriptionExtentionOrder Map_SupscriptionExtentionOrderRequest_To_SupscriptionExtentionOrder(this SupscriptionExtentionOrderRequest supscriptionExtentionOrderRequest)
        {
            return new SupscriptionExtentionOrder
            {
                UserId = supscriptionExtentionOrderRequest.UserId.Trim(),
                TierId = supscriptionExtentionOrderRequest.TierId.Trim(),
                TransactionId = supscriptionExtentionOrderRequest.TransactionId.Trim(),
            };
        }

        public static GetProfileResponse ToGetProfileResponse(this Teacher teacher)
        {
            return new GetProfileResponse()
            {
                UserId = teacher.UserId.Trim(),
                FullName = teacher.FullName.Trim(),
                Address = teacher.Address.Trim(),
                Phone = teacher.Phone.Trim(),
                YearOfBirth = teacher.YearOfBirth,
                TierName = teacher.Tier.TierName.Trim(),
            };
        }

        public static GetProfileResponse ToGetProfileResponse(this Student student)
        {
            return new GetProfileResponse()
            {
                UserId = student.UserId.Trim(),
                FullName = student.FullName.Trim(),
                Address = student.Address.Trim(),
                Phone = student.Phone.Trim(),
                YearOfBirth = student.YearOfBirth,
                TierName = student.Tier.TierName.Trim(),
            };
        }

        public static UpdateProfileView ToUpdateProfileView(this Teacher teacher)
        {
            return new UpdateProfileView()
            {
                UserId = teacher.UserId.Trim(),
                FullName = teacher.FullName.Trim(),
                Address = teacher.Address.Trim(),
                Phone = teacher.Phone.Trim(),
                YearOfBirth = teacher.YearOfBirth,
                TierName = teacher.Tier.TierName.Trim(),
            };
        }

        public static UpdateProfileView ToUpdateProfileView(this Student student)
        {
            return new UpdateProfileView()
            {
                UserId = student.UserId.Trim(),
                FullName = student.FullName.Trim(),
                Address = student.Address.Trim(),
                Phone = student.Phone.Trim(),
                YearOfBirth = student.YearOfBirth,
                TierName = student.Tier.TierName.Trim(),
            };
        }

        public static GetMiniGameResponse ToGetMiniGameResponse(this Minigame minigame)
        {
            return new GetMiniGameResponse()
            {
                MinigameId = minigame.MinigameId.Trim(),
                MinigameName = minigame.MinigameName.Trim(),
                ThumbnailImage = minigame.ThumbnailImage.Trim(),
                TeacherId = minigame.TeacherId.Trim(),
                TeacherName = minigame.Teacher.FullName.Trim(),
                CourseId = minigame.CourseId.Trim(),
                DataText = minigame.DataText.Trim(),
                Duration = minigame.Duration,
                ParticipantsCount = minigame.ParticipantsCount,
                RatingScore = minigame.RatingScore,
                TemplateId = minigame.TemplateId.Trim(),
                TemplateName = minigame.Template?.TemplateName.Trim() ?? "",
            };
        }

        public static GetAllMinigameModel ToGetAllMinigameModel(this Minigame minigame)
        {
            return new GetAllMinigameModel()
            {
                MinigameId = minigame.MinigameId.Trim(),
                MinigameName = minigame.MinigameName.Trim(),
                ThumbnailImage = minigame.ThumbnailImage.Trim(),
                TeacherId = minigame.TeacherId.Trim(),
                TeacherName = minigame.Teacher.FullName.Trim(),
                CourseId = minigame.CourseId.Trim(),
                Duration = minigame.Duration,
                ParticipantsCount = minigame.StudentAccomplisments.Count(),
                RatingScore = minigame.Ratings.Any() ? minigame.Ratings.Average(x => x.Score) : 0d,
                TemplateId = minigame.TemplateId.Trim(),
                TemplateName = minigame.Template.TemplateName.Trim()
            };
        }

        public static AddMiniGameResponseModel ToAddMiniGameResponseModel(this Minigame minigame)
        {
            return new AddMiniGameResponseModel()
            {
                MinigameId = minigame.MinigameId.Trim(),
                MinigameName = minigame.MinigameName.Trim(),
                TeacherId = minigame.TeacherId.Trim(),
                Duration = minigame.Duration,
                TemplateId = minigame.TemplateId.Trim(),
                TemplateName = minigame.Template.TemplateName.Trim(),
                CourseId = minigame.CourseId.Trim(),
                ThumbnailImage = minigame.ThumbnailImage.Trim()
            };
        }
        public static DetailCoursesResponse? Map_Course_To_DetailCoursesResponse(this Course course)
        {
            var mappedCourse = new DetailCoursesResponse
            {
                CourseId = course.CourseId.Trim(),
                CourseName = course.CourseName.Trim(),
                DataText = course.DataText.Trim(),
                LevelId = course.LevelId.Trim(),
                LevelName = course.Level.LevelName.Trim(),
            };

            return mappedCourse;
        }

        public static List<CoursesResponse> Map_ListCourse_To_ListCoursesResponse(this List<Course> list)
        {
            var mappedList = new List<CoursesResponse>();
            foreach (var item in list)
            {
                var response = new CoursesResponse
                {
                    CourseId = item.CourseId.Trim(),
                    CourseName = item.CourseName.Trim(),
                    LevelId = item.LevelId.Trim(),
                    LevelName = item.Level.LevelName.Trim(),
                };

                mappedList.Add(response);
            }

            return mappedList;
        }
        public static Course Map_UpdateCourseRequest_To_Course(this UpdateCourseRequest request)
        {
            var course = new Course
            {
                CourseId = request.CourseId.Trim(),
                CourseName = request.CourseName.Trim(),
                DataText = request.DataText.Trim(),
                LevelId = request.LevelId.Trim(),
            };

            return course;
        }

        public static UpdateMiniGameModel ToUpdateMiniGameResponseModel(this Minigame minigame)
        {
            return new UpdateMiniGameModel()
            {
                MinigameId = minigame.MinigameId.Trim(),
                MinigameName = minigame.MinigameName.Trim(),
                TeacherId = minigame.TeacherId.Trim(),
                Duration = minigame.Duration,
                TemplateId = minigame.TemplateId.Trim(),
                TemplateName = minigame.Template.TemplateName.Trim(),
                CourseId = minigame.CourseId.Trim(),
                ThumbnailImage = minigame.ThumbnailImage.Trim()
            };
        }

        public static GetMinigameTemplatesModel ToGetMinigameTemplateModel(this MinigameTemplate template)
        {
            return new GetMinigameTemplatesModel()
            {
                TemplateId = template.TemplateId.Trim(),
                TemplateName = template.TemplateName.Trim(),
                Description = template.Description.Trim(),
                Image = template.Image.Trim(),
                Summary = template.Summary.Trim(),
            };
        }

        public static Rating Map_CreateRatingRequest_To_Rating(this CreateRatingRequest request)
        {
            return new Rating()
            {
                StudentId = request.StudentId.Trim(),
                MinigameId = request.MinigameId.Trim(),
                Score = request.Score,
                Comment = request.Comment,
                CreatedDate = DateTime.UtcNow
            };
        }

        public static List<GetAllRatingResponse> Map_ListRating_To_ListGetAllRatingResponse(this List<Rating> list)
        {
            var mappedList = new List<GetAllRatingResponse>();

            foreach (Rating rating in list)
            {
                var ratingResponse = new GetAllRatingResponse
                {
                    StudentId = rating.StudentId.Trim(),
                    StudentName = rating.Student.FullName.Trim(),
                    MinigameId = rating.MinigameId.Trim(),
                    Score = rating.Score,
                    Comment = rating.Comment,
                    CreatedDate = rating.CreatedDate
                };

                mappedList.Add(ratingResponse);
            }

            return mappedList;
        }

        public static AccomplishmentViewModel ToViewModel(this StudentAccomplisment accomplisment)
        {
            return new AccomplishmentViewModel
            {
                StudentId = accomplisment.StudentId.Trim(),
                StudentName = accomplisment.Student?.FullName.Trim() ?? "",
                MinigameId = accomplisment.MinigameId.Trim(),
                Score = accomplisment.Score,
                Duration = accomplisment.Duration,
                TakenDate = accomplisment.TakenDate
            };
        }
    }
}
