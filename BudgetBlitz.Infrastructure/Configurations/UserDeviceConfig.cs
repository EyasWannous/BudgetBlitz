using BudgetBlitz.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetBlitz.Infrastructure.Configurations;

public class UserDeviceConfig : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.HasKey(userDevice => userDevice.Id);

        builder.Property(userDevice => userDevice.DeviceToken)
            .IsRequired();

        builder.HasOne(userDevice => userDevice.User)
            .WithMany(user => user.Devices)
            .HasForeignKey(userDevice => userDevice.UsertId)
            .IsRequired();

        builder.ToTable("UserDevices");
    }
}
