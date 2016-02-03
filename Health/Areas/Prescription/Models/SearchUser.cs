using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Health.Areas.Users.Models;
using HealthDB.Model;

namespace Health.Areas.Prescription.Models
{
    public class SearchUser
    {

        [EmailAddress(ErrorMessage = "Silahkan masukkan Email dengan format yang benar")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Nama Depan")]
        public string FirstName { get; set; }

        [Display(Name = "Nama Keluarga")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Tanggal Lahir")]
        public string DOB { get; set; }

        public int? UserTypeId { get; set; }
        public List<User> Users { get;set;}

        public int PatientID { get; set; }
        public int DoctorID { get; set; }
    }
}