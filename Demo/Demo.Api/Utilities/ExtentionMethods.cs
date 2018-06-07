using System;
using System.Threading.Tasks;
using System.Linq;
using Helpa.Api.Models;

namespace Helpa.Api.Utilities
{
    public static class ExtentionMethods
    {
        public static async Task<Locations> AddLocationAsync(Locations location)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Set<Locations>().Add(location);
                await context.SaveChangesAsync();
                return location;
            }
        }

        public static async Task<int> UpdateLocationAsync(Locations location)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Set<Locations>().Attach(location);
                context.Entry(location).State = System.Data.Entity.EntityState.Modified;
                int result = await context.SaveChangesAsync();
                return result;
            }
        }

        public static Locations FindLocationAsync(string LocationName)
        {
            using (var context = new ApplicationDbContext())
            {
                var location = context.Set<Locations>().Where(x => x.LocationName == LocationName).FirstOrDefault();
                return location;
            }
        }

        public static ApplicationUser FindByPhoneNumberAsync(string phoneNumber)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefault();
                return user;
            }
        }

        public static bool IsLocationAvalable(string LocationName)
        {
            using (var context = new ApplicationDbContext())
            {
                var location = context.Set<Locations>().Where(x => x.LocationName == LocationName).FirstOrDefault();
                if (location == null)
                    return false;
                return true;
            }
        }

        public static async Task<UserLog> AddUserLogAsync(UserLog userLog)
        {
            userLog.RowStatus = "I";
            userLog.CreatedDate = DateTime.UtcNow;
            using (var context = new ApplicationDbContext())
            {
                context.Set<UserLog>().Add(userLog);
                await context.SaveChangesAsync();
                return userLog;
            }
        }
    }
}