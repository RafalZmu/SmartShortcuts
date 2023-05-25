using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartShortcuts.Models
{
    public class Shortcut
    {
        public List<Key> ShortcutKeys { get; set; }
        public string? Action { get; set; }
        public string ShortcutToDisplay { get; set; }
        public string ID { get; set; }

        #region Public Constructors

        public Shortcut()
        {
        }

        public Shortcut(List<Key> shortcutKeys, string action)
        {
            ID = Guid.NewGuid().ToString();
            ShortcutKeys = shortcutKeys;
            Action = action;
            ShortcutToDisplay = string.Join("+", shortcutKeys.Select(x => x.KeyName).ToList());
        }

        #endregion Public Constructors
    }
}