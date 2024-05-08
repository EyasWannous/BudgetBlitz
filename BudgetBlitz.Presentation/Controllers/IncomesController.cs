using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Presentation.DTO.Income;
using BudgetBlitz.Presentation.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBlitz.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncomesController(IUnitOfWork unitOfWork) : BaseContoller(unitOfWork)
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var incomes = await _unitOfWork.Incomes.GetAllAsync();

        return Ok(incomes.Select(income => income.Outgoing()));
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var income = await _unitOfWork.Incomes.GetByIdAsync(id);
        if (income is null)
            return NotFound();

        return Ok(income.Outgoing());
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IncomingIncomeDTO incomingIncomeDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var income = await _unitOfWork.Incomes.AddAsync(incomingIncomeDTO.Incoming());

        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetById), new { id = income.Id }, income.Outgoing());
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var income = await _unitOfWork.Incomes.GetByIdAsync(id);
        if (income is null)
            return NotFound();

        await _unitOfWork.Incomes.DeleteAsync(income);

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] IncomingIncomeDTO incomingIncomeDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var income = await _unitOfWork.Incomes.GetByIdAsync(id);
        if (income is null)
            return NotFound();

        var hasUser = await _unitOfWork.Users.GetByIdAsync(incomingIncomeDTO.UserId);
        if (hasUser is null || hasUser.Id != income.UserId)
            return BadRequest();

        income.Date = incomingIncomeDTO.Date;
        income.Amount = incomingIncomeDTO.Amount;
        income.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CompleteAsync();

        return Ok(income.Outgoing());
    }

}
