using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace SmartShortcuts.Models
{
    public class ShortcutsContext : DbContext
    {
        public DbSet<Shortcut> Shortcuts { get; set; }
        public DbSet<Action> Actions { get; set; }

        public string DbPath { get; }

        #region Public Constructors

        public ShortcutsContext(string DbOptionalPath = null)
        {
            if (DbOptionalPath is null)
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                var separator = System.IO.Path.DirectorySeparatorChar;
                if (!File.Exists($"{path}{separator}SmartShortcuts"))
                {
                    Directory.CreateDirectory(path + separator + "SmartShortcuts");
                }
                DbPath = $"{path}{separator}SmartShortcuts{separator}shortcuts.db";
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