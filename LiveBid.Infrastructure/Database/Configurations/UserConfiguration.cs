using LiveBid.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace LiveBid.Infrastructure.Database.Configurations
{

    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");


            builder.Property(user => user.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(user => user.DisplayName)
                .HasColumnName("display_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(user=>user.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(255)
                .IsRequired();


            builder.Property(user => user.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(user => user.UpdatedAt)
                .HasColumnName("updated_at");

            builder.HasIndex(user => user.Email)
            .IsUnique();




        }
    }
}
