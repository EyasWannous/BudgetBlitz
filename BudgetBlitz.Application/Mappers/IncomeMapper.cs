using BudgetBlitz.Application.DTO.Income;
using BudgetBlitz.Domain.Models;

namespace BudgetBlitz.Application.Mappers;

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
