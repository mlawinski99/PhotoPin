using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace ids.Services
{
    public class AspNetIdentityProfileService : IProfileService
    {
        protected UserManager<IdentityUser> _userManager;

        public AspNetIdentityProfileService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
        };

            context.IssuedClaims.AddRange(claims);

            //Return
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            //Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;

            context.IsActive = (user != null) && ((!user.LockoutEnd.HasValue) || (user.LockoutEnd.Value <= DateTime.Now));

            //Return
            return Task.FromResult(0);
        }
    }
}
