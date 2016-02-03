using System.Data.Linq;
using System.Data.Linq.Mapping;
using HealthDB.Model;

namespace HealthDB.Context
{
    [Database]
    public class PrescriptionContext : DataContext
    {
        public static string strConnection { get; set; }
        public PrescriptionContext() : base("Server=sqlexp-ore.crit7zacwo0v.us-west-2.rds.amazonaws.com,1433; Database=HealthcareDB;uid = webuser; password=Webuser02;")
        {
        }

        public Table<Prescription> Prescriptions;
        public Table<PrescriptionType> PrescriptionTypes;
        public Table<DrugName> DrugNames;
        public Table<PrescriptionStatus> PrescriptionStatuses;
    }
}