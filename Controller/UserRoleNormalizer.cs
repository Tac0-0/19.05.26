using Doner.Data.Entities;
using Doner.Data.Enum;

namespace Doner.Controller;

internal static class UserRoleNormalizer
{
    public static bool NormalizeLegacyRole(Users user)
    {
        if (Enum.IsDefined(typeof(UserRole), user.Role))
        {
            return false;
        }

        UserRole? normalizedRole = user switch
        {
            Admins => UserRole.Admin,
            Customers => UserRole.Customer,
            Employees => UserRole.Employee,
            _ => null
        };

        if (normalizedRole is null)
        {
            return false;
        }

        user.Role = normalizedRole.Value;
        return true;
    }

    public static bool NormalizeLegacyRoles(IEnumerable<Users> users)
    {
        bool changed = false;
        foreach (Users user in users)
        {
            changed |= NormalizeLegacyRole(user);
        }

        return changed;
    }
}
