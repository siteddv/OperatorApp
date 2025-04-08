using Microsoft.EntityFrameworkCore;
using OperatorApp.Core.Dtos;
using OperatorApp.Core.Entities;
using OperatorApp.Core.Interfaces;
using OperatorApp.Infrastructure.Data;

namespace OperatorApp.Infrastructure.Repositories;

public class OperatorRepository : IOperatorRepository
{
    private readonly AppDbContext _context;

    public OperatorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Operator>> GetAllAsync()
    {
        return await _context.Operators
            .AsNoTracking()
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<Operator?> GetByIdAsync(int code)
    {
        return await _context.Operators
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Code == code)
            .ConfigureAwait(false);
    }

    public async Task AddAsync(OperatorDto dto)
    {
        var entity = new Operator() { Name = dto.Name };
        await _context.Operators.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(Operator entity)
    {
        _context.Operators.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(int code)
    {
        var entity = await _context.Operators.FindAsync(code).ConfigureAwait(false);
        if (entity != null)
        {
            _context.Operators.Remove(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}