using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartShortcuts.Models
{
    public class WindowManager
    {
        public static Process? FindClosestProcess(Process[] processes, string targetWindowTitle)
        {
            int maxMatchingLength = 0;
            Process closestProcess = null;

            foreach (Process process in processes)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    File.AppendAllText("log.txt", "2");
                    File.AppendAllText("log.txt", process.MainWindowTitle.ToString());
                    if (process.MainModule is null)
                    {
                        File.AppendAllText("log.txt", "Null");
                    }
                    else
                    {
                        File.AppendAllText("log.txt", "Not Null");
                    }
                    File.AppendAllText("log.txt", process.MainModule + "-------\n");
                    int matchingLength = GetProcessMatchingLength(process.MainModule.ModuleName, targetWindowTitle);

                    if (matchingLength > maxMatchingLength && matchingLength > (targetWindowTitle.Length / 2) && process.MainModule.ModuleName.Length < targetWindowTitle.Length * 2)
                    {
                        maxMatchingLength = matchingLength;
                        closestProcess = process;
                    }
                }
            }
            File.AppendAllText("log.txt", closestProcess?.MainModule.ModuleName + "\n");

            return closestProcess;
        }

        public static int GetProcessMatchingLength(string possibleMatch, string target)
        {
            int matchingLength = 0;
            possibleMatch = possibleMatch.ToLower().Replace(" ", "");
            target = target.ToLower().Replace(" ", "");

            for (int i = 1; i < target.Length; i++)
            {
                if (possibleMatch.Contains(target[..i]))
                    matchingLength = i;
                else
                    break;
            }
            return matchingLength;
        }

        public static void LaunchMatchingProgram(Shortcut shortcut)
        {
            if (shortcut.Actions.Count == 0 || shortcut.Actions is null)
                return;

            Process[] oppenedProceses = Process.GetProcesses()
                .Where(x => !string.IsNullOrEmpty(x.MainWindowTitle))
                .ToArray();

            foreach (var action in shortcut.Actions)
            {
                File.AppendAllText("log.txt", action.Path + "\n");
                if (Directory.Exists(action.Path))
                {
                    Process? explorerProcess = oppenedProceses.FirstOrDefault(x => x.MainModule.ModuleName == "explorer.exe");
                    if (explorerProcess is null)
                    {
                        Process proc = new();
                        proc.StartInfo.FileName = "explorer.exe";
                        proc.StartInfo.Arguments = action.Path;
                        proc.Start();
                    }
                    else
                    {
                        IntPtr handle = explorerProcess.MainWindowHandle;
                        ShowWindow(handle, 4);
                        SetForegroundWindow(handle);
                    }
                    continue;
                }
                File.AppendAllText("log.txt", "1");

                Process? processToOpen = FindClosestProcess(oppenedProceses, action.Path.Split(@"\").Last().Replace(".exe", ""));
                if (processToOpen is null)
                {
                    try
                    {
                        Process proc = new();
                        proc.StartInfo.FileName = action.Path;
                        proc.Start();
                    }
                    catch (Exception) { }
                }
                else
                {
                    IntPtr handle = processToOpen.MainWindowHandle;

                    Windowplacement placement = new();
                    GetWindowPlacement(handle, ref placement);

                    if (placement.showCmd == 2)
                    {
                        ShowWindow(handle, 9);
                    }
                    else
                    {
                        if (handle == GetForegroundWindow())
                        {
                            ShowWindow(handle, 6);
                            return;
                        }
                    }
                    SetForegroundWindow(handle);
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref Windowplacement lpwndpl);

        private struct Windowplacement
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }
    }
}