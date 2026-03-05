using GtCores.IdentityCore;
using Microsoft.EntityFrameworkCore;

namespace GSites.Extensions;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : IdentityDbContext<User, Role, UserClaim, UserRole, UserLogin, RoleClaim, UserToken, UserPasskey>(options)
{

}
