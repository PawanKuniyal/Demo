using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Helpa.Api.Models;
using Microsoft.Owin;
using Helpa.Api.Utilities;

namespace Helpa.Api.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            _publicClientId = publicClientId ?? throw new ArgumentNullException("publicClientId");
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            string deviceId = context.OwinContext.Get<string>("deviceId");

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            if (user.Id > 0 && deviceId != null)
            {
                UserLog userLog = new UserLog
                {
                    UserId = user.Id,
                    DeviceId = deviceId
                };
                await ExtentionMethods.AddUserLogAsync(userLog);
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var claims = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.Role);
            var roles = Newtonsoft.Json.JsonConvert.SerializeObject(claims.Select(x => x.Value));

            AuthenticationProperties properties = CreateProperties(user.Id, user.UserName, user.ProfileImage, roles);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            string deviceId = string.Empty;
            if(context.Parameters.Where(f => f.Key == "deviceId").Select(f => f.Value).SingleOrDefault()[0] != null)
            {
                deviceId = context.Parameters.Where(f => f.Key == "deviceId").Select(f => f.Value).SingleOrDefault()[0];
                context.OwinContext.Set<string>("deviceId", deviceId);
                context.Validated();
            }
            else
            {
                context.Rejected();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(int userId, string userName, string profileImage, string Roles)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "id", userId.ToString() },
                { "roles", Roles }
            };

            if (!String.IsNullOrEmpty(profileImage))
            {
                data.Add("profileImage", profileImage);
            }
            
            return new AuthenticationProperties(data);
        }
    }
}