using System.Linq;

namespace SmartShortcuts.Services
{
    public interface IProjectRepository
    {
        #region Public Methods

        IQueryable<T> GetByID<T>(string ID) where T : class;

        IQueryable<T> GetAll<T>() where T : class;

        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        void Save();

        void Update<T>(T entiry) where T : class;

        #endregion Public Methods
    }
}