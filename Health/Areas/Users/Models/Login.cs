using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Health.Helper;
using System.Web.Mvc;

namespace Health.Areas.Users.Models
{
    public class Login
    {
        public string UserID { get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Email Anda")]
        [EmailAddress(ErrorMessage = "Silahkan masukkan Email dengan format yang benar")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Password Anda")]
        [StringLength(10, ErrorMessage = "Minimum 6 karakter dan Maximum 10 karakter", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Result { get; set; }

    }
}