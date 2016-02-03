using System.Data.Linq.Mapping;

namespace HealthDB.Model
{
    [Table(Name = "PrescriptionType")]
    public class PrescriptionType
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string Name { get; set; }
    }
}
