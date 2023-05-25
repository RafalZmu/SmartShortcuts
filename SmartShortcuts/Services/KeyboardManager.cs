using SmartShortcuts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SmartShortcuts.Services
{
    public class KeyboardManager
    {
        #region Fields

        private readonly IProjectRepository _database;

        #endregion Fields

        #region Public Constructors

        public KeyboardManager(IProjectRepository database)
        {
            _database = database;

            Task getKeys = Task.Run(() => GetAllCurrentlyPressedKeys());
        }

        #endregion Public Constructors

        #region Events

        public event EventHandler<KeyEventArgs> KeyPressed;

        #endregion Events

        #region Private Methods

        private void GetAllCurrentlyPressedKeys()
        {
            Dictionary<int, string> keyDictionary = new();
            _database.GetAll<Key>()
                .ToList()
                .ForEach(x => keyDictionary.Add(x.VKCode, value: x.KeyName));

            [DllImport("user32.dll")]
            static extern short GetAsyncKeyState(int vKey);

            List<string> clieckedKeys = new();
            while (true)
            {
                clieckedKeys.Clear();
                Thread.Sleep(50);
                foreach (var item in keyDictionary)
                {
                    short keyStatus = GetAsyncKeyState(item.Key);
                    bool isKeyPressed = ((keyStatus >> 15) & 0x0001) == 0x0001;
                    bool unprocessedKey = (keyStatus >> 0 & 0x0001) == 0x0001;

                    if (isKeyPressed)
                    {
                        clieckedKeys.Add(item.Value);
                    }
                }
                if (clieckedKeys.Count > 0)
                {
                    KeyPressed?.Invoke(this, new KeyEventArgs(clieckedKeys));
                }
            }
        }

        #endregion Private Methods
    }

    public class KeyEventArgs : EventArgs
    {
        #region Properties

        public List<string> Keys { get; set; }

        #endregion Properties

        #region Public Constructors

        public KeyEventArgs(List<string> keys)
        {
            Keys = keys;
        }

        #endregion Public Constructors
    }
}