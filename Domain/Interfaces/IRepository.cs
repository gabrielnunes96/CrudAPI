using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> InsertASync(T item);
        Task<T> UpdateASync(T item);
        Task<bool> DeleteASync(Guid id);
        Task<T> SelectASync(Guid id);
        Task<IEnumerable<T>> SelectASync();
        Task<bool> ExistASync(Guid id);
    }
}
