using SportsShop.Core.Entities;
using SportsShop.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T: BaseEntity
    {

        Task<IQueryable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<IQueryable<T>> GetAllWithSpecAsync(ISpecifications<T> spec);

        Task<T?> GetWithSpecAsync(ISpecifications<T> spec);

        Task<int> GetCountAsync(ISpecifications<T> spec);

        Task<IEnumerable<TResult>> ListOptionalAsync<TResult>(ISpecifications<T, TResult> spec);

        Task<T> AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        bool Exists(int id);

        Task SaveChangesAsync();
    }
}
