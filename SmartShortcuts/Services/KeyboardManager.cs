using SmartShortcuts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SmartShortcuts.Services
{
    public class KeyboardManager
    {
        #region Public Constructors

        public KeyboardManager(IProjectRepository database)
        {
            Task getKeys = Task.Run(() => GetAllCurrentlyPressedKeys());
        }

        #endregion Public Constructors

        #region Events

        public event EventHandler<KeyEventArgs> KeyPressed;

        #endregion Events

        #region Private Methods

        private void GetAllCurrentlyPressedKeys()
        {
            Dictionary<int, string> keyDictionary = GetKeyCodesDictionary();

            [DllImport("user32.dll")]
            static extern short GetAsyncKeyState(int vKey);

            List<string> clickedKeys = new();
            while (true)
            {
                clickedKeys.Clear();
                Thread.Sleep(10);
                foreach (var item in keyDictionary)
                {
                    short keyStatus = GetAsyncKeyState(item.Key);
                    bool isKeyPressed = ((keyStatus >> 15) & 0x0001) == 0x0001;
                    bool unprocessedKey = (keyStatus >> 0 & 0x0001) == 0x0001;

                    if (isKeyPressed)
                    {
                        clickedKeys.Add(item.Value);
                    }
                }
                if (clickedKeys.Count > 0)
                {
                    KeyPressed?.Invoke(this, new KeyEventArgs(clickedKeys));
                }
            }
        }

        private Dictionary<int, string> GetKeyCodesDictionary()
        {
            Dictionary<int, string> returnDictionary = new();
            string projectDirectory = Environment.CurrentDirectory;
            var keyCodesList = File.OpenText($@"{projectDirectory}\Assets\KeyCodes.txt")
                .ReadToEnd()
                .Split("\r\n")
                .ToList();
            keyCodesList.ForEach(x =>
                {
                    string[] split = x.Split("\t");
                    returnDictionary.Add(Convert.ToInt32(split[0].Trim(), 16), split[1].Trim());
                });

            return returnDictionary;
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