using Avalonia.Media;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SmartShortcuts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShortcuts.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private SettingsManager _settingsManager = new SettingsManager();
        private Color _selectedColor;

        [Reactive]
        public string AccentColor { get; set; }

        public Color SelectedColor
        {
            get
            {
                ChangeAccentColor(_selectedColor);
                return _selectedColor;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedColor, value);
            }
        }

        public SettingsViewModel()
        {
            SelectedColor = Color.Parse(_settingsManager.Properties["AccentColor"]);
            AccentColor = _settingsManager.Properties["AccentColor"];
        }

        public event EventHandler AccentColorChanged;

        private void ChangeAccentColor(Color newColor)
        {
            string hex = "#" + newColor.R.ToString("X2") + newColor.G.ToString("X2") + newColor.B.ToString("X2");
            _settingsManager.Properties["AccentColor"] = hex;
            _settingsManager.Save();
            AccentColor = hex;
            AccentColorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}