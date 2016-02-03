using System.ComponentModel;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
namespace Health.EnumType
{
    public enum LicenseType
    {
        Doctor = 1,
        Pharmacist = 2,
        [Display(Name = "Lab Technician")]
        Lab_Technician = 3,
        Nurse = 4
    }
}
