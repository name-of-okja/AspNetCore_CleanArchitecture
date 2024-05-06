using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(
                new ApplicationUser
                {
                    Id = "a9b7a589-8c70-4893-b1c7-6b0e3771e79c",
                    Email = "admin@test.com",
                    Name = "__ad",
                    LastName = "min__",
                    UserName = "__admin__",
                    NormalizedEmail = "admin@test.com".ToUpper(),
                    NormalizedUserName = "__admin__".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "admin123$"),
                    EmailConfirmed = true,
                },
                new ApplicationUser
                {
                    Id = "1f0f37ec-63aa-4a38-9516-e4a4a6706c5e",
                    Email = "test1@test.com",
                    Name = "__te",
                    LastName = "st1__",
                    UserName = "__test1__",
                    NormalizedEmail = "test1@test.com".ToUpper(),
                    NormalizedUserName = "__test1__".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "test1123$"),
                    EmailConfirmed = true,
                }
            );
    }
}
