using System.Data.Linq.Mapping;

namespace HealthDB.Model
{
    [Table(Name = "LocationType")]
    public class LocationType
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string Name { get; set; }
    }
}
