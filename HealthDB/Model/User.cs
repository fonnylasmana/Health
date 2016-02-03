using System;
using System.Data.Linq.Mapping;

namespace HealthDB.Model
{
    [Table(Name ="User")]
    public class User
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public string FirstName { get; set; }

        [Column]
        public string MiddleName { get; set; }

        [Column]
        public string LastName { get; set; }

        [Column]
        public string Email { get; set; }

        [Column]
        public DateTime DOB { get; set; }

        [Column]
        public int UsertypeId { get; set; }
    }
}
