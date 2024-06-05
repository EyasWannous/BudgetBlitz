using BudgetBlitz.Domain.Models;
using BudgetBlitz.Presentation.DTO.Income;

namespace BudgetBlitz.Presentation.Mappers;

public static class IncomeMapper
{
    public static OutgoingIncomesDTO Outgoing(this Income income)
    {
        return new OutgoingIncomesDTO
        {
            Id = income.Id,
            Amount = income.Amount,
            Date = income.Date,
            UserId = income.UserId
        };
    }

    public static Income Incoming(this IncomingIncomeDTO incomingIncomeDTO)
    {
        return new Income
        {
            Amount = incomingIncomeDTO.Amount,
            Date = incomingIncomeDTO.Date,
        };
    }
}
