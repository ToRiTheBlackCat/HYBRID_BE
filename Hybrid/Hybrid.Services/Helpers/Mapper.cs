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
    }
}
