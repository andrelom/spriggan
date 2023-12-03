using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Spriggan.Data.Identity.Contracts.Entities;
using Spriggan.Data.Identity.Contracts.Values;

namespace Spriggan.Data.Identity;

public static class Seeding
{
    private const string TemporaryPassword = "@QwTXb6g2kt2HvDx";

    public static void Seed(ModelBuilder builder)
    {
        SeedRoles(builder);

        SeedAdministratorUser(builder);
    }

    #region Private Methods

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<Role>().HasData(new Role
        {
            Id = Roles.Administrator.Id,
            ConcurrencyStamp = Roles.Administrator.Id.ToString(),
            Name = Roles.Administrator.Name,
            NormalizedName = Roles.Administrator.Name.ToUpperInvariant()
        });

        builder.Entity<Role>().HasData(new Role
        {
            Id = Roles.User.Id,
            ConcurrencyStamp = Roles.User.Id.ToString(),
            Name = Roles.User.Name,
            NormalizedName = Roles.User.Name.ToUpperInvariant()
        });
    }

    private static void SeedAdministratorUser(ModelBuilder builder)
    {
        var hasher = new PasswordHasher<User>();

        var user = new User
        {
            Id = Users.Administrator.Id,
            SecurityStamp = Users.Administrator.Id.ToString(),
            ConcurrencyStamp = Users.Administrator.Id.ToString(),
            UserName = Users.Administrator.Name,
            NormalizedUserName = Users.Administrator.Name.ToUpperInvariant(),
            Email = Users.Administrator.Email,
            NormalizedEmail = Users.Administrator.Email.ToUpperInvariant(),
            EmailConfirmed = true
        };

        user.PasswordHash = hasher.HashPassword(user, TemporaryPassword);

        builder.Entity<User>().HasData(user);

        builder.Entity<UserRole>().HasData(new UserRole
        {
            RoleId = Roles.Administrator.Id,
            UserId = Users.Administrator.Id
        });
    }

    #endregion
}
