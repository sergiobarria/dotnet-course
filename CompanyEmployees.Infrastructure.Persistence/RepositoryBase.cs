using System.Linq.Expressions;
using CompanyEmployees.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Infrastructure.Persistence;

public abstract class RepositoryBase<T>(RepositoryContext repositoryContext) : IRepositoryBase<T>
    where T : class
{
    public IQueryable<T> FindAll(bool trackChanges)
    {
        return !trackChanges ? repositoryContext.Set<T>().AsNoTracking() : repositoryContext.Set<T>();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
    {
        return !trackChanges ? repositoryContext.Set<T>().AsNoTracking() : repositoryContext.Set<T>();
    }

    public void Create(T entity)
    {
        repositoryContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        repositoryContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        repositoryContext.Set<T>().Remove(entity);
    }
}