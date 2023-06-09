using System;
using System.Reactive.Linq;

namespace SmartShortcuts.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase CurrentPage { get; set; }

        public MainWindowViewModel()
        {
            var shortcutsViewModel = new ShortcutsViewModel();
            CurrentPage = shortcutsViewModel;
        }
    }
}