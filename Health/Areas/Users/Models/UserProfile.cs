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
    public class UserProfile
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Email Anda")]
        [EmailAddress(ErrorMessage = "Silahkan masukkan Email dengan format yang benar")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Tanggal Lahir Anda")]
        [DataType(DataType.Date)]
        [Display(Name = "Tanggal Lahir")]
        public string DOB { get; set; }

        [Required(ErrorMessage = "Silahkan pilih Jenis Kelamin Anda")]
        [Display(Name = "Jenis Kelamin")]
        public int GenderID { get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Nama Depan Anda")]
        [Display(Name = "Nama Depan")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Nama Keluarga Anda")]
        [Display(Name = "Nama Keluarga")]
        public string LastName { get; set; }

        [Display(Name = "Nama Tengah")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Password Anda")]
        [StringLength(10, ErrorMessage = "Minimum 6 karakter dan Maximum 10 karakter", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Nomor Telpon HP atau Rumah jika tidak ada")]
        [Display(Name = "Nomor Telpon HP")]
        [DataType(DataType.PhoneNumber)]
        public string MobilePhone { get; set; }

        [Required(ErrorMessage = "Silahkan pilih Profesi Anda")]
        [Display(Name = "Profesi")]
        public int UserTypeID { get; set; }

        [Display(Name = "Registrasi Berlaku Sampai")]
        public string LicenseExpireDate { get; set; }

        [Display(Name = "Tanggal Pengeluaran Registrasi")]
        public string LicenseIssueDate { get; set; }

        [Display(Name = "Tempat Pengeluaran Registrasi")]
        public string LicenseIssuePlace { get; set; }

        [Display(Name = "Nomor Surat Ijin Praktek (SIP)")]
        public string LicenseNumber { get; set; }

        public List<ListboxHelper.CustomListItem> getAllGender { get; set; }
        public List<ListboxHelper.CustomListItem> getAllUserTypes { get; set; }
    }
}