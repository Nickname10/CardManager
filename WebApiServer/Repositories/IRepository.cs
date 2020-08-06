using System.Collections.Generic;

namespace WebApiServer.Repositories
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T item);
        void Update(T newItem);
        void Delete(int id);
        void SaveAsync();
    }
}