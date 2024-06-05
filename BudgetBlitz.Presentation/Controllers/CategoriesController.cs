using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Presentation.DTO.Category;
using BudgetBlitz.Presentation.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBlitz.Presentation.Controllers;

[Authorize]
public class CategoriesController(IUnitOfWork unitOfWork) : BaseContoller(unitOfWork)
{

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();

        return Ok(categories.Select(category => category.Outgoing()));
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category is null)
            return NotFound();

        return Ok(category.Outgoing());
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IncomingCategoryDTO incomingCategoryDTO)
    {
        var category = await _unitOfWork.Categories.AddAsync(incomingCategoryDTO.Incoming());

        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category.Outgoing());
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category is null)
            return NotFound();

        await _unitOfWork.Categories.DeleteAsync(category);

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] IncomingCategoryDTO incomingCategoryDTO)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category is null)
            return NotFound();

        category.Name = incomingCategoryDTO.Name;
        category.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CompleteAsync();

        return Ok(category.Outgoing());
    }
}
