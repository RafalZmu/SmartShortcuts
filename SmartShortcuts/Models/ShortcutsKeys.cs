namespace SmartShortcuts.Models
{
    public class ShortcutsKeys
    {
        public string KeyID { get; set; }
        public string ShortcutID { get; set; }
        public Key Key { get; set; } = null!;
        public Shortcut Shortcut { get; set; } = null!;
    }
}