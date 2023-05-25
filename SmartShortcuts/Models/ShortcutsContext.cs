using Microsoft.EntityFrameworkCore;
using System;

namespace SmartShortcuts.Models
{
    public class ShortcutsContext : DbContext
    {
        public DbSet<Shortcut> Shortcuts { get; set; }

        public string DbPath { get; }

        #region Public Constructors

        public ShortcutsContext(string DbOptionalPath = null)
        {
            if (DbOptionalPath is null)
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}shortcuts.db";
            }
            else
            {
                DbPath = DbOptionalPath;
            }
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        #endregion Protected Methods
    }
}