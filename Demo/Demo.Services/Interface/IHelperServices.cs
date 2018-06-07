using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpa.Entities;
using Helpa.Entities.CustomEntities;

namespace Helpa.Services.Interface
{
    public interface IHelperServices
    {
        IQueryable<HomeHelpers> GetHelpers(int radius = 0, double Latitude = 0, double Longitude = 0);
        IQueryable<AspNetUser> GetAllUsers();
        IQueryable<HelperService> GetAllHelpers();
        IQueryable<HelperService> GetHelperById(int Id);
        IQueryable<HomeHelpers> GetHelperByEmail(string Email);
        IQueryable<HomeHelpers> GetHelperByPhone(string MobileNo);
        Task<HelperService> GetHelperAsync(int Id);
        Task<Helper> AddHelperAsync(Helper helper);
        Task<HelperService> AddHelperServiceAsync(HelperService helperService);
        HelperServiceScope AddHelperServiceScope(HelperServiceScope helperServiceScope);
        Task<HelperServiceScope> AddHelperServiceScopeAsync(HelperServiceScope helperServiceScope);
        Helper GetHelper(int Id);
        int UpdateHelper(Helper helper);
        Task<int> UpdateHelperAsync(Helper helper);
        Task<bool> FindByUserIdAsync(int Id);
        IQueryable<AspNetUser> GetUserByEmail(string Email);
        IQueryable<AspNetUser> GetUserByPhone(string MobileNumber);
        List<object> GetClusteredHelpers(SearchParams searchParams);
        IQueryable<AspNetUserLogin> AspNetUserLogins();

        //Add By Shyam
       int AddLogin(AspNetUserLogin AspNetUserLogin);
        
        //int AddRole(AspNetRole AspNetRole);
    }
}
