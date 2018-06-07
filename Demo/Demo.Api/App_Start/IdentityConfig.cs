using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Helpa.Api.Models;
using System.Threading.Tasks;
using Helpa.Api.Utilities;
using System.Data.Entity.Utilities;

namespace Helpa.Api
{
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store)
            : base(store)
        {
        }

        public override async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            RegexUtilities regexUtilities = new RegexUtilities();
            ApplicationUser user = new ApplicationUser();
            if (regexUtilities.IsValidEmail(userName))
            {
                user = await FindByEmailAsync(userName);
            }
            else if (regexUtilities.IsValidPhoneNumber(userName))
            {
                user = ExtentionMethods.FindByPhoneNumberAsync(userName);
            }

            return await CheckPasswordAsync(user, password).WithCurrentCulture() ? user : null;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new CustomUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
