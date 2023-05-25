using System;
using System.ComponentModel.DataAnnotations;

namespace SmartShortcuts.Models
{
    public class Key
    {
        [Key]
        public string ID { get; set; }

        public Int32 VKCode { get; set; }

        public string KeyName { get; set; }
    }
}