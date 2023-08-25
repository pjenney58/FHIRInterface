using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Support.Model
{
    public class PalisaidUserManager : UserManager<ApplicationUser>
    {
        public PalisaidUserManager(IUserStore<ApplicationUser> store,
                                   IOptions<IdentityOptions> optionsAccessor,
                                   IPasswordHasher<ApplicationUser> passwordHasher,
                                   IEnumerable<IUserValidator<ApplicationUser>> userValidators,
                                   IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
                                   ILookupNormalizer keyNormalizer,
                                   IdentityErrorDescriber errors,
                                   IServiceProvider services,
                                   ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}