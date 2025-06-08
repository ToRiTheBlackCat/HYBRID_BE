#nullable disable
using Hybrid;
using Hybrid.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Profile
{
    public class UpdateProfileRequest
    {
        public string UserId { get; set; }

        public bool IsTeacher { get; set; } = false;

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public int YearOfBirth { get; set; }


        public void AssignValueTo(Student student)
        {
            student.FullName = FullName;
            student.Address = Address;
            student.Phone = Phone;
            student.YearOfBirth = YearOfBirth;
        }

        public void AssignValueTo(Teacher teacher)
        {
            teacher.FullName = FullName;
            teacher.Address = Address;
            teacher.Phone = Phone;
            teacher.YearOfBirth = YearOfBirth;
        }
    }


}
