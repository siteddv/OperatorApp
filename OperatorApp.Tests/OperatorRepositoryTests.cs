using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OperatorApp.Core.Entities;
using OperatorApp.Infrastructure.Data;
using OperatorApp.Core.Interfaces;
using OperatorApp.Infrastructure.Repositories;

namespace OperatorApp.Tests
{
    public class OperatorRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly AppDbContext _context;
        private readonly IOperatorRepository _repository;

        public OperatorRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();

            _repository = new OperatorRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _connection.Close();
        }

        [Fact]
        public async Task AddOperator_UniqueName_Success()
        {
            // Arrange
            var op = new Operator { Code = 1, Name = "Test" };

            // Act
            await _repository.AddAsync(op);

            // Assert
            var result = await _repository.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task AddOperator_DuplicateName_ThrowsException()
        {
            // Arrange
            var op1 = new Operator { Code = 1, Name = "Test" };
            var op2 = new Operator { Code = 2, Name = "Test" }; 

            await _repository.AddAsync(op1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DbUpdateException>(() => _repository.AddAsync(op2));
            Assert.Contains("UNIQUE constraint failed", exception.InnerException?.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateOperator_Success()
        {
            // Arrange
            var op = new Operator { Code = 1, Name = "Test" };
            await _repository.AddAsync(op);

            // Act
            op.Name = "UpdatedName";
            await _repository.UpdateAsync(op);

            // Assert
            var updatedOperator = await _repository.GetByIdAsync(1);
            Assert.NotNull(updatedOperator);
            Assert.Equal("UpdatedName", updatedOperator.Name);
        }

        [Fact]
        public async Task DeleteOperator_Success()
        {
            // Arrange
            var op = new Operator { Code = 1, Name = "Test" };
            await _repository.AddAsync(op);

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var deletedOperator = await _repository.GetByIdAsync(1);
            Assert.Null(deletedOperator);
        }

        [Fact]
        public async Task GetAllOperators_ReturnsAllOperators()
        {
            // Arrange
            var op1 = new Operator { Code = 1, Name = "Operator1" };
            var op2 = new Operator { Code = 2, Name = "Operator2" };
            await _repository.AddAsync(op1);
            await _repository.AddAsync(op2);

            // Act
            var operators = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, operators.Count());
            Assert.Contains(operators, o => o.Code == 1 && o.Name == "Operator1");
            Assert.Contains(operators, o => o.Code == 2 && o.Name == "Operator2");
        }

        [Fact]
        public async Task GetByIdOperator_NotFound()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }
    }
}