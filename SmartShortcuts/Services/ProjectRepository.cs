using Avalonia.FreeDesktop.DBusIme;
using SmartShortcuts.Models;
using System.Linq;

namespace SmartShortcuts.Services
{
    public class ProjectRepository : IProjectRepository
    {
        private ShortcutsContext _context;

        #region Public Constructors

        public ProjectRepository(ShortcutsContext context)
        {
            context.Database.EnsureCreated();
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> GetByID<T>(string ID) where T : class
        {
            return _context.Set<T>().Where(x => x.GetType().GetProperty("ID").GetValue(x).ToString() == ID);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update<T>(T entiry) where T : class
        {
            _context.Set<T>().Update(entiry);
        }

        #endregion Public Methods
    }
}