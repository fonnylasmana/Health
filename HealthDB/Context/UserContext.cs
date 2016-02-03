using System.Data.Linq;
using System.Data.Linq.Mapping;
using HealthDB.Model;

namespace HealthDB.Context
{
    [Database]
    public class UserContext : DataContext
    {
        public static string strConnection { get; set; }
        public UserContext() : base("Server=sqlexp-ore.crit7zacwo0v.us-west-2.rds.amazonaws.com,1433; Database=HealthcareDB;uid = webuser; password=Webuser02;") {
        }

        public Table<User> Users;
        public Table<UserType> UserTypes;
        public Table<Gender> Genders;
        public Table<LocationType> LocationTypes;
    }
}


