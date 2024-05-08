using BudgetBlitz.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetBlitz.Infrastructure.Configurations;

public class ExpenseConfig : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(expense => expense.Id);

        builder.Property(expense => expense.Amount)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(expense => expense.Date)
            .IsRequired();

        builder.HasOne(expense => expense.User)
            .WithMany(user => user.Expenses)
            .HasForeignKey(expense => expense.UserId)
            .IsRequired();

        builder.HasOne(expense => expense.Category)
            .WithMany(category => category.Expenses)
            .HasForeignKey(expense => expense.CategoryId)
            .IsRequired();

        builder.ToTable("Expenses");
    }
}
