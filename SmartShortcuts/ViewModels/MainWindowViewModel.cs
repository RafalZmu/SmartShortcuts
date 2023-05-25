namespace SmartShortcuts.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase Content { get; set; }

        public MainWindowViewModel()
        {
            Content = new ShortcutsViewModel();
        }
    }
}