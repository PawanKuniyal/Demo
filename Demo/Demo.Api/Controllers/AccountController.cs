using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Helpa.Api.Models;
using Helpa.Api.Providers;
using Helpa.Api.Results;
using Helpa.Entities.CustomEntities;
using Helpa.Api.Utilities;
using Helpa.Services.Interface;
using Helpa.Services;
using Helpa.Api.Models.ReturnModels;
using Newtonsoft.Json;
using Helpa.Entities;

namespace Helpa.Api.Controllers
{
    // [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private IHelperServices helperServices = null;
        private IUtilityServices utilityServices;

        public AccountController()
        {
            utilityServices = new UtiltiyServices();
            helperServices = new Services.HelperServices();
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin?.LoginProvider
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        //[Route("ManageInfo")]
        //public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        //{
        //    IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());

        //    if (user == null)
        //    {
        //        return null;
        //    }

        //    List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

        //    foreach (IdentityUserLogin linkedAccount in user.Logins)
        //    {
        //        logins.Add(new UserLoginInfoViewModel
        //        {
        //            LoginProvider = linkedAccount.LoginProvider,
        //            ProviderKey = linkedAccount.ProviderKey
        //        });
        //    }

        //    if (user.PasswordHash != null)
        //    {
        //        logins.Add(new UserLoginInfoViewModel
        //        {
        //            LoginProvider = LocalLoginProvider,
        //            ProviderKey = user.UserName,
        //        });
        //    }

        //    return new ManageInfoViewModel
        //    {
        //        LocalLoginProvider = LocalLoginProvider,
        //        Email = user.UserName,
        //        Logins = logins,
        //        ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
        //    };
        //}

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId<int>(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId<int>());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId<int>(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                var roles = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.Id, user.UserName, user.ProfileImage, Newtonsoft.Json.JsonConvert.SerializeObject(roles.Select(x => x.Value)));
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                string message = "";
                var errors = ModelState.Values;
                foreach (var err in errors)
                    message = message + err.Errors[0].ErrorMessage;

                return BadRequest(message);
            }
            var CheckExitUser = UserManager.FindByEmail(model.Email);

            if (CheckExitUser == null)
            {
                var CheckExitUserName = UserManager.FindByName(model.UserName);
                if (CheckExitUserName == null)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        RowStatus = true,
                        GenderId = 0
                    };

                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                    var IdCreated = new CreatedId();
                    if (result.Succeeded)
                    {
                        var lastAddedUser = await UserManager.FindByNameAsync(user.UserName);
                        int userId = lastAddedUser.Id;
                        await UserManager.AddToRoleAsync(userId, model.Role);
                        IdCreated.Id = userId;
                    }

                    if (!result.Succeeded)
                    {
                        var err = result.Errors.FirstOrDefault();
                        Errors errors = new Errors
                        {
                            ErrorMessage = err
                        };
                        var Messgae = errors.ErrorMessage;
                        return BadRequest(errors.ErrorMessage);
                        //return Content(System.Net.HttpStatusCode.Conflict, errors);
                    }

                    return Ok(new { Code = "200", Message = "Successfully Register", Data = IdCreated.Id });
                }
                string Messge = "User Name Already Exist";
                return BadRequest(Messge);
            }
            return BadRequest("Email Already Exist");
        }

        // PUT api/Account/Register
        [AllowAnonymous]
        [Route("complete-registration/{id}")]
        public async Task<IHttpActionResult> Manage(int id, UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Locations location = new Locations();
            var user = new ApplicationUser();
            //string profileImage = string.Empty;

            user = await UserManager.FindByIdAsync(id);

            if (!string.IsNullOrEmpty(userDTO.ProfileImage))
            {
                string ImageName = FileConverter.UploadFileToFileFolder(userDTO.ProfileImage, "ProfileImages");
                user.ProfileImage = ImageName;
            }

            //if (string.IsNullOrEmpty(userDTO.ProfileImage))
            //{
            //    profileImage = FileConverter.UploadFileToFileFolder(userDTO.ProfileImage, "ProfileImages");
            //}
            if (!String.IsNullOrEmpty(userDTO.UserName))
                user.UserName = userDTO.UserName;

            user.GenderId = userDTO.GenderId;

            if (!String.IsNullOrEmpty(userDTO.MobileNumber))
                user.PhoneNumber = userDTO.MobileNumber;

            if (!String.IsNullOrEmpty(userDTO.Email))
                user.Email = userDTO.Email;

            if (!String.IsNullOrEmpty(userDTO.Description))
                user.Description = userDTO.Description;

            //user.ProfileImage = profileImage;

            if (!String.IsNullOrEmpty(userDTO.Latitude) && !String.IsNullOrEmpty(userDTO.Longitude))
            {
                var point = LocationPoint.CreatePoint(Convert.ToDouble(userDTO.Latitude), Convert.ToDouble(userDTO.Longitude));
                location.LocationName = userDTO.LocationName;
                location.LocationGeography = point;
                location.RowStatus = true;
                location.CreatedDate = DateTime.UtcNow;
            }

            Locations rs;
            bool locationAvailable = ExtentionMethods.IsLocationAvalable(location.LocationName);

            if (locationAvailable!=false)
            {
                rs = await ExtentionMethods.AddLocationAsync(location);
                user.LocationId = rs.LocationId;
            }
            else
            {
                rs = ExtentionMethods.FindLocationAsync(location.LocationName);
                if(rs!=null)
                user.LocationId = rs.LocationId;
            }

            IdentityResult result = await UserManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { Code = "200", Message = "Registration Successfully ", ID = user.Id });
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [Route("get-parent-user")]
        public IHttpActionResult GetUser(string Email = "", string MobileNumber = "")
        {
            dynamic user;
            if (!String.IsNullOrEmpty(Email))
            {
                user = helperServices.GetUserByEmail(Email);
                return Ok(user);
            }
            else if (!String.IsNullOrEmpty(MobileNumber))
            {
                user = helperServices.GetUserByPhone(MobileNumber);
                return Ok(user);
            }
            else
            {
                return NotFound();
            }

        }

        // POST api/Account/RegisterExternal
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            int userId = 0;
            if (!ModelState.IsValid)
            {
                string message = "";
                var errors = ModelState.Values;
                foreach (var err in errors)
                    message = message + err.Errors[0].ErrorMessage;

                return BadRequest(message);
            }
            var usercheckExist = UserManager.FindByEmail(model.Email);
            if (usercheckExist == null)
            {
                var usercheckusername = UserManager.FindByName(model.UserName);
                if (usercheckusername == null)
                {
                    //var usercheckPhoneNo = helperServices.GetAllUsers().Where(x => x.PhoneNumber == model.PhoneNumber).FirstOrDefault();
                    //if (usercheckPhoneNo == null)
                    //{
                        var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, RowStatus = true };
                        IdentityResult result = await UserManager.CreateAsync(user);
                        var IdCreated = new CreatedId();
                        if (result.Succeeded)
                        {
                            var lastAddedUser = await UserManager.FindByNameAsync(user.UserName);
                            userId = lastAddedUser.Id;
                            await UserManager.AddToRoleAsync(userId, model.Role);
                            IdCreated.Id = userId;
                            int UserId = helperServices.GetAllUsers().OrderByDescending(x => x.Id).FirstOrDefault().Id;
                            var UserLogin = new CustomUserLogin() { UserId = UserId, ProviderKey = model.LoginProvider };

                            AspNetUserLogin UserData = new AspNetUserLogin();
                            UserData.LoginProvider = model.LoginProvider;
                            UserData.UserId = UserLogin.UserId;

                            UserData.ProviderKey = model.Token.Substring(0, 120);
                            int Id = helperServices.AddLogin(UserData);
                            //return Ok(new { Code = "200", Message = "Success", Data = userId });
                        }


                        return Ok(new { Code = "200", Message = "Success", Data = IdCreated.Id });
                    }
                //    return BadRequest("Phone No Already Exit");
                //}
                string Messge = "User Name Already Exit";
                return BadRequest(Messge);
            }
            return BadRequest("Email Already Exit");

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
