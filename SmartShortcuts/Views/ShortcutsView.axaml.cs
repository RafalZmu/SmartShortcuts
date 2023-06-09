using Avalonia.Controls;
using Avalonia.Input;
using SmartShortcuts.ViewModels;
using System;
using System.Linq;

namespace SmartShortcuts.Views;

public partial class ShortcutsView : UserControl
{
    public ShortcutsView()
    {
        InitializeComponent();
        DataContext = new ShortcutsViewModel();

        AddHandler(DragDrop.DropEvent, Drop);
    }

    private void Drop(object sender, DragEventArgs e)
    {
        if (!e.Data.Contains(DataFormats.FileNames))
            return;
        Action.Text = string.Join('\n', e.Data.GetFileNames());
    }
}