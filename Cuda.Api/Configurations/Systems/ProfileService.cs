using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuda.Model.Entities;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace Cuda.Api.Configurations.Systems
{
    public class ProfileService : IProfileService
    {
        protected UserManager<Account> _userManager;
        private readonly IUserClaimsPrincipalFactory<Account> _claimsFactory;

        public ProfileService(UserManager<Account> userManager, IUserClaimsPrincipalFactory<Account> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            // lấy ra các claims được request
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // thêm claim tùy thích
            claims.AddRange(new List<Claim>
            {
                new Claim("fullName", user.FirstName + " " + user.LastName),
                new Claim("userName", user.UserName),
            });

            context.IssuedClaims = claims;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;

            context.IsActive = (user != null) && user.IsActive;

            //>Return
            return Task.FromResult(0);
        }
    }
}