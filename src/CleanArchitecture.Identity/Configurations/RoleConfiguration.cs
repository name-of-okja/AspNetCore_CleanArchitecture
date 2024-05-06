using CleanArchitecture.Identity.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
                new IdentityRole
                {
                    Id = "9eeb3f74-ba13-430d-939b-7989d0922436",
                    Name = ApplicationUserRoles.Admin,
                    NormalizedName = ApplicationUserRoles.Admin.ToUpper(),
                },
                new IdentityRole
                {
                    Id = "3bb881ee-71b7-445c-bb66-ecc2b6fe9498",
                    Name = ApplicationUserRoles.Operator,
                    NormalizedName = ApplicationUserRoles.Operator.ToUpper(),
                }
            );
    }
}
