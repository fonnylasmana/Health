using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Health.Helper;
using System.Web.Mvc;
using Health.Areas.Users.Models;
using HealthDB.Model;

namespace Health.Areas.Prescription.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        public string InvoiceId { get; set; }

        [Display(Name = "Pasien")]
        public int PatientUserId { get; set; }
        public User Patient { get; set; }

        [Display(Name = "Apoteker")]
        public int PharmacistUserId { get; set; }
        public User Pharmacist { get; set; }

        [Display(Name = "Status")]
        public int PrescriptionStatusId { get; set; }

        [Display(Name = "Resep dikeluarkan oleh")]
        public int PrescribedByUserId { get; set; }
        public User  Doctor { get; set; }

        [Display(Name = "Jenis Resep")]
        public int PrescriptionTypeId { get; set; }

        [Display(Name = "Instruksi")]
        public string Direction { get; set; }

        [Display(Name = "Resep Obat")]
        public string DrugName { get; set; }

        [Display(Name = "Nama Racikan")]
        public string GroupName { get; set; }

        [Display(Name = "Jumlah")]
        public int Quantity { get; set; }

        [Display(Name = "Dosis")]
        public string Strength { get; set; }

        public List<Drug> Drugs { get; set; }

        public List<ListboxHelper.CustomListItem> getAllDrugs { get; set; }
        public List<ListboxHelper.CustomListItem> getAllPickupStatus { get; set; }
        public List<ListboxHelper.CustomListItem> getAllPrescriptionTypes { get; set; }
    }

    public class Drug
    {
        public int Id { get; set; }

        [Display(Name = "Jenis Resep")]
        public string PrescriptionType{ get; set; }

        [Display(Name = "Instruksi")]
        public string Direction { get; set; }

        [Display(Name = "Resep Obat")]
        public string DrugName { get; set; }

        [Display(Name = "Nama Racikan")]
        public string GroupName { get; set; }

        [Display(Name = "Jumlah")]
        public int Quantity { get; set; }

        [Display(Name = "Dosis")]
        public string Strength { get; set; }
    }


}