using System.Data.Linq.Mapping;

namespace HealthDB.Model
{
    [Table(Name = "UserType")]
    public class UserType
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string Name { get; set; }
    }
}