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
        private ShortcutsViewModel _shortcutsViewModel { get; set; }
        private SettingsViewModel _settingsViewModel { get; set; }

        public MainWindowViewModel()
        {
            _shortcutsViewModel = new ShortcutsViewModel();
            _settingsViewModel = new SettingsViewModel();
            CurrentPage = _shortcutsViewModel;

            OpenSettingsControlCommand = ReactiveCommand.Create(() => { CurrentPage = _settingsViewModel; });
            OpenShortcutsControlCommand = ReactiveCommand.Create(() => { CurrentPage = _shortcutsViewModel; });

            _settingsViewModel.AccentColorChanged += SettingsViewModel_AccentColorChanged;
        }

        private void SettingsViewModel_AccentColorChanged(object? sender, EventArgs e)
        {
            _shortcutsViewModel.AccentColor = _settingsViewModel.AccentColor;
        }
    }
}