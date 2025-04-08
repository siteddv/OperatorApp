using Microsoft.EntityFrameworkCore;
using OperatorApp.Core.Entities;

namespace OperatorApp.Core.Interfaces;

public interface IAppDbContext
{
    public DbSet<Operator> Operators { get; set; }
}