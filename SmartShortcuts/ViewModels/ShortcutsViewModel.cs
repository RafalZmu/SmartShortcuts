using Avalonia.Controls;
using AvaloniaEdit.Utils;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SmartShortcuts.Models;
using SmartShortcuts.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Action = SmartShortcuts.Models.Action;
using System.Text.Json;

namespace SmartShortcuts.ViewModels
{
    public class ShortcutsViewModel : ViewModelBase
    {
        #region Fields

        private readonly KeyboardManager _keyboardManager;
        private readonly SettingsManager _settingsManager;
        private IProjectRepository _database;
        private HashSet<string> _keysPressed = new();
        private List<string> LastKeysPressed = new();

        private ObservableCollection<Shortcut> _shortcuts;

        #endregion Fields

        #region Properties

        private string _selectedShortcutAction;

        public ObservableCollection<Shortcut> Shortcuts
        {
            get
            {
                return _shortcuts;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _shortcuts, value);
            }
        }

        public ReactiveCommand<Unit, Unit> CreateNewShortcutCommand { get; }
        public ReactiveCommand<Unit, Unit> StartListeningToKeysCommand { get; }
        public ReactiveCommand<string, Unit> ChangeShortcutInFocusCommand { get; }
        public ReactiveCommand<Window, Task> OpenFileBrowserCommand { get; }
        public ReactiveCommand<string, Unit> DeleteShortcutCommand { get; }
        public string AccentColor { get; set; } = "#066D08";

        [Reactive]
        public string SelectedShortcutID { get; set; }

        [Reactive]
        public string SelectedShortcutKeys { get; set; }

        public string SelectedShortcutAction
        {
            get => _selectedShortcutAction;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedShortcutAction, value);
                UpdateDatabase(SelectedShortcutID);
            }
        }

        public bool ListeningToKeys { get; set; }

        #endregion Properties

        public ShortcutsViewModel()
        {
            _database = new ProjectRepository(new ShortcutsContext());

            Shortcuts = new ObservableCollection<Shortcut>(_database.GetAll<Shortcut>().ToList());

            var actions = _database.GetAll<Action>().ToList();
            foreach (var shortcut in Shortcuts)
            {
                shortcut.Actions = actions.Where(x => x.Shortcut.ID == shortcut.ID).ToList();
            }
            SelectedShortcutAction = "";
            SelectedShortcutKeys = "";

            CreateNewShortcutCommand = ReactiveCommand.Create(CreateNewShortcut);
            ChangeShortcutInFocusCommand = ReactiveCommand.Create<string, Unit>(ChangeShortcutInFocus);
            OpenFileBrowserCommand = ReactiveCommand.Create<Window, Task>(OpenFileBrowser);
            StartListeningToKeysCommand = ReactiveCommand.Create(() => { ListeningToKeys = true; });
            DeleteShortcutCommand = ReactiveCommand.Create<string>(DeleteShortcut);

            _keyboardManager = new KeyboardManager(_database);
            _keyboardManager.KeyPressed += ListenToKeysPressed;

            _settingsManager = new SettingsManager();
            AccentColor = _settingsManager.Properties["AccentColor"];
        }

        private void DeleteShortcut(string id)
        {
            Shortcut shortcutToDelete = _database.GetByID<Shortcut>(id);
            List<Action> actionsToDelete = _database.GetAll<Action>().Where(x => x.Shortcut.ID == id).ToList();
            foreach (Action action in actionsToDelete)
            {
                _database.Delete(action);
            }
            _database.Delete(shortcutToDelete);
            _database.Save();
            Shortcuts.Remove(shortcutToDelete);
        }

        private async Task OpenFileBrowser(Window window)
        {
            var fileDialog = new OpenFileDialog();
            var result = await fileDialog.ShowAsync(window);
            if (result != null)
            {
                string[] fileNames = result;
            }
        }

        private void CreateNewShortcut()
        {
            var newShortcut = new Shortcut();
            _database.Add<Shortcut>(newShortcut);
            _database.Save();
            Shortcuts.Add(newShortcut);
            ChangeShortcutInFocus(newShortcut.ID);
            SelectedShortcutAction = "New shortcut";
            SelectedShortcutKeys = "";
        }

        #region Private Methods

        private void UpdateDatabase(string id)
        {
            var shortcutToEdit = Shortcuts.Where(x => x.ID == id).FirstOrDefault();
            if (shortcutToEdit == null)
                return;
            var actionsToDelete = _database.GetAll<Action>().Where(x => x.Shortcut.ID == shortcutToEdit.ID).ToList();
            actionsToDelete.ForEach(x => _database.Delete(x));

            shortcutToEdit.Actions = SelectedShortcutAction.Trim().Split('\n').Select(x => new Action() { Path = x }).ToList();
            shortcutToEdit.ShortcutKeys = SelectedShortcutKeys.Replace(" ", "");

            foreach (var action in shortcutToEdit.Actions)
            {
                _database.Add(action);
            }
            _database.Update<Shortcut>(shortcutToEdit);
            _database.Save();
        }

        private Unit ChangeShortcutInFocus(string shortcutID)
        {
            var shortcut = Shortcuts.FirstOrDefault(x => x.ID == shortcutID);
            if (shortcut == null)
                return new Unit();
            SelectedShortcutID = shortcutID;
            SelectedShortcutKeys = shortcut.ShortcutKeys;
            var actionPaths = Shortcuts.Where(x => x.ID == SelectedShortcutID).SelectMany(s => s.Actions).Select(a => a.Path);
            SelectedShortcutAction = string.Join('\n', actionPaths);
            return new Unit();
        }

        private void ListenToKeysPressed(object sender, KeyEventArgs clickedKeys)
        {
            if (clickedKeys == null)
                return;

            var pressedKeys = new List<string>();
            foreach (var key in clickedKeys.Keys)
            {
                pressedKeys.Add(key.Replace(" key", ""));
            }
            if (pressedKeys.SequenceEqual(LastKeysPressed))
            {
                return;
            }
            LastKeysPressed = pressedKeys;

            if (ListeningToKeys)
            {
                if (pressedKeys.Contains("ENTER"))
                {
                    _keysPressed.Remove("ENTER");
                    var shortcut = Shortcuts.Where(x => x.ID == SelectedShortcutID).FirstOrDefault();
                    if (shortcut == null)
                        return;
                    shortcut.ShortcutKeys = string.Join('+', _keysPressed);

                    // Updates the UI
                    Shortcuts.Replace(shortcut, shortcut);

                    UpdateDatabase(SelectedShortcutID);
                    ListeningToKeys = false;
                    _keysPressed.Clear();
                    return;
                }
                _keysPressed.AddRange<string>(pressedKeys);
                SelectedShortcutKeys = string.Join("+", _keysPressed);
            }
            else
            {
                OpenWindowBasedOnClickedKeys(pressedKeys);
            }
        }

        private void OpenWindowBasedOnClickedKeys(List<string> clickedKeys)
        {
            // Check all Shortcuts for matching keys combination
            foreach (var shortcut in Shortcuts)
            {
                if (clickedKeys.Count != shortcut.ShortcutKeys.Split('+').Count())
                    continue;

                bool allKeysMatch = clickedKeys.All(x => shortcut.ShortcutKeys.Split('+').Any(y => y == x));

                if (allKeysMatch)
                    WindowManager.LaunchMatchingProgram(shortcut);
            }
        }
    }

    #endregion Private Methods
}