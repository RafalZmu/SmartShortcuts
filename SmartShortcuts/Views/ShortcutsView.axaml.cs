using Avalonia.Controls;
using Avalonia.Input;
using SmartShortcuts.ViewModels;
using System;
using System.Linq;
using Shell32;
using IWshRuntimeLibrary;
using System.Collections.Generic;
using Avalonia.Interactivity;

namespace SmartShortcuts.Views;

public partial class ShortcutsView : UserControl
{
    public ShortcutsView()
    {
        InitializeComponent();
        //DataContext = new ShortcutsViewModel();

        AddHandler(DragDrop.DropEvent, Drop);
    }

    //Changes the app focus to the label when the flyout is closed to prevent the user from activating the button again
    private void Flyout_Closed(object sender, EventArgs e)
    {
        FocusLabel.Focus();
    }

    private void Drop(object sender, DragEventArgs e)
    {
        List<string> paths = Action.Text.Split('\n').ToList();
        if (!e.Data.Contains(DataFormats.FileNames))
            return;
        var filePaths = e.Data.GetFileNames();
        foreach (var filePath in filePaths)
        {
            if (filePath.Contains(".lnk"))
            {
                WshShell shell = new();
                WshShortcut shortcut = (WshShortcut)shell.CreateShortcut(filePath);
                paths.Add(shortcut.TargetPath);
            }
            else
                paths.Add(filePath);
        }

        Action.Text = string.Join('\n', paths);
    }
}