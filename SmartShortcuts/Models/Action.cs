using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShortcuts.Models
{
    public class Action : BaseDataObject
    {
        public string Path { get; set; }
        public Shortcut Shortcut { get; set; }

        [NotMapped]
        public Process Process { get; set; }

        public Action()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}