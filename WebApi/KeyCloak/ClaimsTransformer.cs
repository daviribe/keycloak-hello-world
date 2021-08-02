using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace WebApi.KeyCloak
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal claimsPrincipal)
        {
            var claimsPrincipalIdentity = (ClaimsIdentity) claimsPrincipal.Identity;

            if (claimsPrincipalIdentity is not {IsAuthenticated: true} || !claimsPrincipalIdentity.HasClaim(claim => claim.Type == "realm_access"))
                return Task.FromResult(claimsPrincipal);
            {
                var realmAccessClaim = claimsPrincipalIdentity.FindFirst(claim => claim.Type == "realm_access");

                var realmAccessAsDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(realmAccessClaim?.Value ??
                    throw new InvalidOperationException("Realm access claim doesn't exist."));

                if (realmAccessAsDict?["roles"] == null) return Task.FromResult(claimsPrincipal);

                foreach (var role in realmAccessAsDict["roles"])
                    claimsPrincipalIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return Task.FromResult(claimsPrincipal);
        }
    }
}