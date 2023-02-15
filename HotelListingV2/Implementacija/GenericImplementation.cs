using HotelListingV2.Data;
using HotelListingV2.Interfejsi;
using Microsoft.EntityFrameworkCore;

namespace HotelListingV2.Implementacija
{
    public class GenericImplementation<T> : IGenericInterface<T>  where T : class
    {
        private readonly HotelListingDbContext context;

        public GenericImplementation(HotelListingDbContext context)
        {
            this.context = context;
        }
        public async Task<T> CreateAsync(T entity)
        {
            await context.AddAsync<T>(entity);
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity= await GetAsync(id);
            context.Remove<T>(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> Exixts(int id)
        {
            return (await GetAsync(id)) == null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            var entity= await context.FindAsync<T>(id);
            return entity;
        }

        public async Task UpdateAsync( T entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
