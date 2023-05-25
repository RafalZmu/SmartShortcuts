using ReactiveUI;
using SmartShortcuts.Models;
using SmartShortcuts.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices;

namespace SmartShortcuts.ViewModels
{
    public class ShortcutsViewModel : ViewModelBase
    {
        #region Fields

        private readonly KeyboardManager _keyboardManager;
        private IProjectRepository _database;
        private string _keyPressed;
        private List<Key> _keys;

        private ObservableCollection<Shortcut> _shortcuts;

        #endregion Fields

        #region Properties

        public ObservableCollection<Shortcut> Shortcuts
        {
            get
            {
                UpdateDatabase(_shortcuts);
                return _shortcuts;
                //Save changes to database
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _shortcuts, value);
            }
        }

        public string KeyPressed
        {
            get => _keyPressed;
            set
            {
                _keyPressed = value;
                this.RaiseAndSetIfChanged(ref _keyPressed, value);
            }
        }

        public ReactiveCommand<Unit, Unit> AddNewShortcutCommand { get; set; }

        #endregion Properties

        #region Public Constructors

        public ShortcutsViewModel()
        {
            _database = new ProjectRepository(new ShortcutsContext());

            _keys = _database.GetAll<Key>().ToList();

            Shortcuts = new ObservableCollection<Shortcut>(_database.GetAll<Shortcut>().ToList());

            AddNewShortcutCommand = ReactiveCommand.Create(() => AddSpaceForNewShortcut());

            _keyboardManager = new KeyboardManager(_database);
            _keyboardManager.KeyPressed += OpenWindowBasedOnClickedKeys;
        }

        #endregion Public Constructors

        #region Private Methods

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void UpdateDatabase(ObservableCollection<Shortcut> shortcuts)
        {
            foreach (var shortcut in shortcuts)
            {
                //Check if property keysToDisplay or Action are empty
                if (shortcut.ShortcutToDisplay == "" || shortcut.Action == "")
                {
                    _database.Delete(shortcut);
                    continue;
                }
                //Check if shortcut is already in database if not add it
                if (!_database.GetAll<Shortcut>().Any(x => x.ID == shortcut.ID))
                {
                    _database.Add(shortcut);
                }
                else
                {
                    //Check if shortcut has changed
                    if (_database.GetAll<Shortcut>()
                            .Any(x => x.ID == shortcut.ID && x.ShortcutToDisplay != shortcut.ShortcutToDisplay))
                    {
                        _database.Update(shortcut);
                    }
                }
            }
        }

        private void AddSpaceForNewShortcut()
        {
            Shortcuts.Add(new Shortcut()
            {
                ID = Guid.NewGuid().ToString(),
                ShortcutToDisplay = "Add new shortcut",
                Action = "",
                ShortcutKeys = new List<Key>()
            });
        }

        private void OpenWindowBasedOnClickedKeys(object sender, KeyEventArgs clickedKeys)
        {
            // Check all Shortcuts for matching keys combination
            foreach (var shortcut in Shortcuts)
            {
                if (clickedKeys.Keys.Count != shortcut.ShortcutKeys.Count)
                {
                    continue;
                }
                bool allKeysMatch = clickedKeys.Keys.All(x => shortcut.ShortcutKeys.Any(y => y.KeyName == x));
                if (allKeysMatch)
                {
                    LaunchMatchingProgram(shortcut);
                }
            }

            static void LaunchMatchingProgram(Shortcut shortcut)
            {
                var runningProcess = Process.GetProcessesByName(shortcut.Action.Split(@"\").Last().Replace(".exe", ""));

                if (runningProcess.Length == 0)
                    Process.Start(shortcut.Action);
                else
                {
                    foreach (var process in runningProcess)
                    {
                        IntPtr handle = process.MainWindowHandle;
                        SetForegroundWindow(handle);
                    }
                }
            }
        }

        #endregion Private Methods
    }
}