using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace Support.Model
{
    public static class JwtTenantId
    {
        public static Guid Get(HttpRequest Request)
        {
            if (Request.Headers.TryGetValue("Authorization", out StringValues authToken))
            {
                string? authHeader = authToken.FirstOrDefault();
                string? jwt = authHeader?.Substring("Bearer ".Length).Trim();
                var jwtToken = new JwtSecurityToken(jwt);

                foreach (var i in jwtToken.Payload)
                {
                    if (i.Key.ToLower().Contains("primarysid"))
                    {
                        return Guid.Parse(i.Value.ToString());
                    }
                }
            }

            return default;
            //throw new InvalidDataException("JwtToken Incomplete");
        }
    }
}