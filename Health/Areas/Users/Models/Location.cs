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
    public class Location
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Harap Isi Kota")]
        [Display(Name = "Kota")]
        public string City { get; set; }

        public int Latitude { get; set; }
        public int Longitude{ get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Propinsi")]
        [Display(Name = "Propinsi")]
        public string Province{ get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Kode Pos")]
        [Display(Name = "Kode Pos")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Silahkan masukkan Alamat")]
        [Display(Name = "Alamat")]
        public string Street1 { get; set; }

        [Display(Name = " ")]
        public string Street2 { get; set; }

        public string Email { get; set; }

        [Display(Name = "Tipe Lokasi")]
        public int LocationTypeID { get; set; }

        [Display(Name = "Nama Lokasi")]
        public string LocName { get; set; }

        [Display(Name = "Fax")]
        [DataType(DataType.PhoneNumber)]
        public string LocFax { get; set; }

        [Display(Name = "Nomor Telpon")]
        [DataType(DataType.PhoneNumber)]
        public string LocPhoneMain { get; set; }

        public List<ListboxHelper.CustomListItem> getAllLocationTypes { get; set; }
    }
}