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
        private ShortcutsViewModel shortcutsViewModel { get; set; }
        private SettingsViewModel settingsViewModel { get; set; }

        public MainWindowViewModel()
        {
            shortcutsViewModel = new ShortcutsViewModel();
            settingsViewModel = new SettingsViewModel();
            CurrentPage = shortcutsViewModel;

            OpenSettingsControlCommand = ReactiveCommand.Create(() => { CurrentPage = settingsViewModel; });
            OpenShortcutsControlCommand = ReactiveCommand.Create(() => { CurrentPage = shortcutsViewModel; });

            settingsViewModel.AccentColorChanged += SettingsViewModel_AccentColorChanged;
        }

        private void SettingsViewModel_AccentColorChanged(object? sender, EventArgs e)
        {
            shortcutsViewModel.AccentColor = settingsViewModel.AccentColor;
        }
    }
}