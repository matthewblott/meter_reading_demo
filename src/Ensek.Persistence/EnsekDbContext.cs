namespace Ensek.Persistence
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;
  using System.Reflection;
  using System.Threading;
  using System.Threading.Tasks;
  using Application.Common.Interfaces;
  using Common;
  using Domain.Entities;
  using FluentValidation;
  using FluentValidation.Results;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata;

  public class EnsekDbContext : DbContext, IEnsekDbContext, IDbContextTransaction
  {
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }
    public DbSet<Role> Groups { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Territory> Territories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserGroups { get; set; }
    private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _currentTransaction;

    public EnsekDbContext(DbContextOptions<EnsekDbContext> options) : base(options)
    {
    }

    public EnsekDbContext(
      DbContextOptions<EnsekDbContext> options,
      ICurrentUserService currentUserService,
      IDateTime dateTime)
      : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      // Validation
      foreach (var entry in ChangeTracker.Entries())
      {
        var vt = typeof (AbstractValidator<>);
        var evt = vt.MakeGenericType(entry.Entity.GetType());  // entry.Metadata.Name
        var validatorTypes =
          Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(evt)).ToList();

        var failures = validatorTypes
          .Select(vt0 =>
          {
            var instance = Activator.CreateInstance(vt0);

            if (instance == null)
            {
              return new ValidationResult();
            }
            
            var validator = (IValidator) instance;
            var  validationResult = validator.Validate(entry.Entity);

            return validationResult;

          })
          .SelectMany(result => result.Errors)
          .Where(f => f != null)
          .ToList();

        if (failures.Count != 0)
        {
          throw new ValidationException(failures);
        }
      }
      
      return base.SaveChangesAsync(cancellationToken);
      
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(EnsekDbContext).Assembly);
    }
    
    public async Task BeginTransactionAsync()
    {
      if (_currentTransaction != null)
      {
        return;
      }

      _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync()
    {
      try
      {
        await SaveChangesAsync().ConfigureAwait(false);

        if (_currentTransaction != null)
        {
          await _currentTransaction!.CommitAsync();
        }
      }
      catch
      {
        RollbackTransaction();
        throw;
      }
      finally
      {
        if (_currentTransaction != null)
        {
          _currentTransaction.Dispose();
          _currentTransaction = null;
        }
      }
    }

    public void RollbackTransaction()
    {
      try
      {
        _currentTransaction?.Rollback();
      }
      finally
      {
        if (_currentTransaction != null)
        {
          _currentTransaction.Dispose();
          _currentTransaction = null;
        }
      }
    }
    
    public IEnumerable<IProperty> Keys(Type type) 
      => Model.FindEntityType(type).FindPrimaryKey().Properties.ToList();
    
  }
  
}