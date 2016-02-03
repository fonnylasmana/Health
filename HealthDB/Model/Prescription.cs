using System.Data.Linq.Mapping;

namespace HealthDB.Model
{
    [Table(Name = "Prescription")]
    public class Prescription
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public int PatientId { get; set; }
        
        [Column]
        public int DoctorId { get; set; }
	
        [Column]
	    public int PharmacistId { get; set; }
        
	    [Column]
        public int PrescriptionStatusId { get; set; }

        [Column]
        public int PrescriptionTypeId { get; set; }

        [Column]
        public string GroupName { get; set; }

        [Column]
        public string DrugName { get; set; }

        [Column]
        public string Instruction { get; set; }

        [Column]
        public int Quantity { get; set; }

        [Column]
        public string Dosage { get; set; }

        [Column]
        public string Invoice { get; set; }
    }
}

