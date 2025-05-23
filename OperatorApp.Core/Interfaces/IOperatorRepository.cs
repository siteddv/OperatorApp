using OperatorApp.Core.Dtos;
using OperatorApp.Core.Entities;

namespace OperatorApp.Core.Interfaces;

public interface IOperatorRepository
{
    Task<IEnumerable<Operator>> GetAllAsync();
    Task<Operator?> GetByIdAsync(int code);
    Task AddAsync(OperatorDto entity);
    Task UpdateAsync(Operator entity);
    Task DeleteAsync(int code);
}