using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Spriggan.Core.Data;

public static class Mapping
{
    public static void Configure<T>(EntityTypeBuilder<T> builder) where T : Entity
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id).ValueGeneratedOnAdd();

        builder.Property(entity => entity.CreatedDate).IsRequired();

        builder.Property(entity => entity.UpdatedDate).IsRequired();
    }
}
