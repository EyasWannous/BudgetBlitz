using BudgetBlitz.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetBlitz.Infrastructure.Configurations;

public class IncomeConfig : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.HasKey(income => income.Id);

        builder.Property(income => income.Amount)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(income => income.Date)
            .IsRequired();

        builder.HasOne(income => income.User)
            .WithMany(user => user.Incomes)
            .HasForeignKey(income => income.UserId)
            .IsRequired();

        builder.ToTable("Incomes");
    }
}
