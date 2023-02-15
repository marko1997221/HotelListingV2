namespace HotelListingV2.Interfejsi
{
    public interface IGenericInterface<T> where T : class
    {
        public Task<List<T>> GetAllAsync();
        public Task<T> GetAsync(int? id);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(int id);
        public Task<T> CreateAsync(T entity);
        public Task<bool> Exixts(int id);
    }
}
