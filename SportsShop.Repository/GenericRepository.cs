using Microsoft.EntityFrameworkCore;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ShopContext _shopContext;

        public GenericRepository(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            return  _shopContext.Set<T>().Where(x => !x.IsDeleted).AsNoTrackingWithIdentityResolution();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var data = await GetAllAsync();
            return await data.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<T> AddAsync(T entity)
        {
            await _shopContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            _shopContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _shopContext.SaveChangesAsync();
        }
    }
}
