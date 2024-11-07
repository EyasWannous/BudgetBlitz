using BudgetBlitz.Domain.Models;
using BudgetBlitz.Application.DTO.Category;

namespace BudgetBlitz.Application.Mappers;

public static class CategoryMapper
{
    public static OutgoingCategoryDTO Outgoing(this Category category)
    {
        return new OutgoingCategoryDTO
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public static Category Incoming(this IncomingCategoryDTO incomingCategoryDTO)
    {
        return new Category
        {
            Name = incomingCategoryDTO.Name
        };
    }
}
