using Microsoft.AspNetCore.Mvc;
using OperatorApp.Core.Dtos;
using OperatorApp.Core.Entities;
using OperatorApp.Core.Interfaces;

namespace OperatorApp.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperatorsController : ControllerBase
{
    private readonly IOperatorRepository _repository;

    public OperatorsController(IOperatorRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var operators = await _repository.GetAllAsync();
        return Ok(operators);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetById(int code)
    {
        var @operator = await _repository.GetByIdAsync(code);
        if (@operator == null)
        {
            return NotFound();
        }

        return Ok(@operator);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OperatorDto dto)
    {
        await _repository.AddAsync(dto);
        return CreatedAtAction(nameof(GetAll), null);
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> Update(int code, [FromBody] Operator? @operator)
    {
        if (@operator == null || code != @operator.Code)
        {
            return BadRequest();
        }

        var existingOperator = await _repository.GetByIdAsync(code);
        if (existingOperator == null)
        {
            return NotFound();
        }

        await _repository.UpdateAsync(@operator);
        return NoContent();
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(int code)
    {
        var existingOperator = await _repository.GetByIdAsync(code);
        if (existingOperator == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(code);
        return NoContent();
    }
}