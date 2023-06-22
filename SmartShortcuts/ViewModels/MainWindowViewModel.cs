using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace SmartShortcuts.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive]
        public ViewModelBase CurrentPage { get; set; }

        public ReactiveCommand<Unit, Unit> OpenSettingsControlCommand { get; set; }
        public ReactiveCommand<Unit, Unit> OpenShortcutsControlCommand { get; set; }

        public MainWindowViewModel()
        {
            var shortcutsViewModel = new ShortcutsViewModel();
            var settingsViewModel = new SettingsViewModel();
            CurrentPage = shortcutsViewModel;

            OpenSettingsControlCommand = ReactiveCommand.Create(() => { CurrentPage = settingsViewModel; });
            OpenShortcutsControlCommand = ReactiveCommand.Create(() => { CurrentPage = shortcutsViewModel; });
        }
    }
}