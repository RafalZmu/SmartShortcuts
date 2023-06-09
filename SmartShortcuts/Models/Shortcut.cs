using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartShortcuts.Models
{
    public class Shortcut : BaseDataObject
    {
        public string ShortcutKeys { get; set; }
        public virtual ICollection<Action> Actions { get; set; }

        #region Public Constructors

        public Shortcut()
        {
            ID = Guid.NewGuid().ToString();
            ShortcutKeys = "New shortcut";
            Actions = new List<Action>();
        }

        #endregion Public Constructors
    }
}