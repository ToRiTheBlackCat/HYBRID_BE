using Azure.Core;
using Hybrid.Repositories.Models;
using Hybrid.Services.ViewModel;
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
                CourseId= minigame.CourseId.Trim(),
                Duration = minigame.Duration,
                ParticipantsCount = minigame.ParticipantsCount,
                RatingScore = minigame.RatingScore,
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
    }
}
