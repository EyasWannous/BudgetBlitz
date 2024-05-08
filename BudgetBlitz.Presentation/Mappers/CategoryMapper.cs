using BudgetBlitz.Domain.Models;
using BudgetBlitz.Presentation.DTO.Category;

namespace BudgetBlitz.Presentation.Mappers;

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
