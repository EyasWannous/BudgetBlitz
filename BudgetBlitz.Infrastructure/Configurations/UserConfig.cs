//using BudgetBlitz.Domain.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace BudgetBlitz.Infrastructure.Configurations;

//public class UserConfig : IEntityTypeConfiguration<User>
//{
//    public void Configure(EntityTypeBuilder<User> builder)
//    {
//        builder.HasKey(user => user.Id);

//        builder.Property(user => user.Username)
//            .IsRequired();

//        builder.Property(user => user.Password)
//            .IsRequired();

//        builder.HasMany(user => user.Incomes)
//            .WithOne(income => income.User)
//            .HasForeignKey(income => income.UserId)
//            .IsRequired();

//        builder.HasMany(user => user.Expenses)
//            .WithOne(expenses => expenses.User)
//            .HasForeignKey(expenses => expenses.UserId)
//            .IsRequired();

//        builder.ToTable("Users");
//    }
//}
