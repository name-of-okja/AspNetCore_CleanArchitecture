using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations;
public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "9eeb3f74-ba13-430d-939b-7989d0922436",
                    UserId = "a9b7a589-8c70-4893-b1c7-6b0e3771e79c",
                },
                new IdentityUserRole<string>
                {
                    RoleId = "3bb881ee-71b7-445c-bb66-ecc2b6fe9498",
                    UserId = "1f0f37ec-63aa-4a38-9516-e4a4a6706c5e",
                }
            );
    }
}
