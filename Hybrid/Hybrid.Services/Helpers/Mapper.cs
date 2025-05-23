using Azure.Core;
using Hybrid.Repositories.Models;
using Hybrid.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Helpers
{
    public static class Mapper
    {
        public static User Map_SignUpVM_To_User(this SignUpRequest signUpRequestViewModel)
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

        public static Student Map_SignUpVM_To_Student(this SignUpRequest signUpRequestViewModel)
        {
            return new Student
            {
                FullName = signUpRequestViewModel.FullName,
                Address = signUpRequestViewModel.Address,
                Phone = signUpRequestViewModel.Phone,
                YearOfBirth = signUpRequestViewModel.YearOfBirth,
                TierId = signUpRequestViewModel.TierId,
            };
        }

        public static Teacher Map_SignUpVM_To_Teacher(this SignUpRequest signUpRequestViewModel)
        {
            return new Teacher
            {
                FullName = signUpRequestViewModel.FullName,
                Address = signUpRequestViewModel.Address,
                Phone = signUpRequestViewModel.Phone,
                YearOfBirth = signUpRequestViewModel.YearOfBirth,
                TierId = signUpRequestViewModel.TierId,
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
    }
}
