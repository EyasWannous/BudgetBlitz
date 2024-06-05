using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Presentation.DTO.Expense;
using BudgetBlitz.Presentation.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace BudgetBlitz.Presentation.Controllers;

[Authorize]
public class ExpensesController(IUnitOfWork unitOfWork,UserManager<User> userManager) : BaseContoller(unitOfWork)
{
    private readonly UserManager<User> _userManager = userManager;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var expenses = await _unitOfWork.Expenses.GetAllAsync();

        return Ok(expenses.Select(expense => expense.Outgoing()));
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
        if (expense is null)
            return NotFound();

        return Ok(expense.Outgoing());
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IncomingExpenseDTO incomingExpenseDTO)
    {
        var hasCategory = await _unitOfWork.Categories.GetByIdAsync(incomingExpenseDTO.CategoryId);
        if (hasCategory is null)
            return BadRequest();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
            return BadRequest();

        var newExpense = incomingExpenseDTO.Incoming();

        newExpense.UserId = user.Id;

        var expense = await _unitOfWork.Expenses.AddAsync(newExpense);

        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense.Outgoing());
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
        if (expense is null)
            return NotFound();

        await _unitOfWork.Expenses.DeleteAsync(expense);

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] IncomingExpenseDTO incomingExpenseDTO)
    {
        var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
        if (expense is null)
            return NotFound();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
            return BadRequest();

        var hasUser = await _unitOfWork.Users.GetByIdAsync(expense.UserId);
        if (hasUser is null || hasUser.Id != user.Id)
            return BadRequest();

        var hasCategory = await _unitOfWork.Categories.GetByIdAsync(expense.CategoryId);
        if (hasCategory is null || hasCategory.Id != incomingExpenseDTO.CategoryId)
            return BadRequest();

        expense.Date = incomingExpenseDTO.Date;
        expense.Amount = incomingExpenseDTO.Amount;
        expense.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CompleteAsync();

        return Ok(expense.Outgoing());
    }
}
