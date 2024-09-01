using BudgetBlitz.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBlitz.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController(IUnitOfWork unitOfWork) : ControllerBase
{
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
}
