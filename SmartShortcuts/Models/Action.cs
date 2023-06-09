using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShortcuts.Models
{
    public class Action : BaseDataObject
    {
        public string Path { get; set; }
        public Shortcut Shortcut { get; set; }

        public Action()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}