using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Presentation.DTO.Income;
using BudgetBlitz.Presentation.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBlitz.Presentation.Controllers;

[Authorize]
public class IncomesController(IUnitOfWork unitOfWork, UserManager<User> userManager) : BaseController(unitOfWork)
{
    private readonly UserManager<User> _userManager = userManager;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var incomes = await _unitOfWork.Incomes.GetAllAsync();

        return Ok(incomes.Select(income => income.Outgoing()));
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var income = await _unitOfWork.Incomes.GetByIdAsync(id);
        if (income is null)
            return NotFound();

        return Ok(income.Outgoing());
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IncomingIncomeDTO incomingIncomeDTO)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
            return BadRequest();
        
        var newIncome = incomingIncomeDTO.Incoming();

        newIncome.UserId = user.Id;

        var income = await _unitOfWork.Incomes.AddAsync(newIncome);

        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetById), new { id = income.Id }, income.Outgoing());
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
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
        var income = await _unitOfWork.Incomes.GetByIdAsync(id);
        if (income is null)
            return NotFound();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
            return BadRequest();

        var hasUser = await _unitOfWork.Users.GetByIdAsync(income.UserId);
        if (hasUser is null || hasUser.Id != user.Id)
            return BadRequest();

        income.Date = incomingIncomeDTO.Date;
        income.Amount = incomingIncomeDTO.Amount;
        income.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CompleteAsync();

        return Ok(income.Outgoing());
    }

}
