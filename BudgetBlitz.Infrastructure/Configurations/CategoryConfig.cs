using BudgetBlitz.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetBlitz.Infrastructure.Configurations;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .IsRequired();

        builder.HasMany(category => category.Expenses)
            .WithOne(expense => expense.Category)
            .HasForeignKey(expense => expense.CategoryId)
            .IsRequired();

        builder.ToTable("Categories");
    }
}
