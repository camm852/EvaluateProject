using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EvaluationProjects.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T Add(T entity);
        void AddRange(IEnumerable<T> entity);
        T? GetId(int id);
        Task<T?> GetIdAsync(int id);
        T? Get(Expression<Func<T, bool>> predicate);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        int Count();
        Task<int> CountAsync();
        void Update(T entity);
        void Remove(T entity);
        void Dispose();
    }
}
