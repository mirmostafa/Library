using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Mohammad.Wpf.Windows.Dialogs
{
    public class BrowseForFolderDialog
    {
        public CommonOpenFileDialog Dialog { get; set; }
        public string DefaultDirectory { get { return this.Dialog.DefaultDirectory; } set { this.Dialog.DefaultDirectory = value; } }
        public string Title { get { return this.Dialog.Title; } set { this.Dialog.Title = value; } }
        public IEnumerable<string> SelectedFolders { get { return this.Dialog.FileNames; } }
        public string SelectedFolder { get { return this.Dialog.FileName; } }
        public BrowseForFolderDialog() { this.Dialog = new CommonOpenFileDialog {IsFolderPicker = true}; }

        public static bool? ShowDialog(out string selectedFolder, string title = null, string defaultDirectory = null)
        {
            var dialog = new BrowseForFolderDialog {DefaultDirectory = defaultDirectory, Title = title};

            var result = dialog.ShowDialog();
            selectedFolder = result == true ? dialog.SelectedFolder : null;
            return result;
        }

        public static bool? ShowDialog(out IEnumerable<string> selectedFolders, string title = null, string defaultDirectory = null)
        {
            var dialog = new BrowseForFolderDialog {DefaultDirectory = defaultDirectory, Title = title};

            var result = dialog.ShowDialog();
            selectedFolders = result == true ? dialog.SelectedFolders : null;
            return result;
        }

        public bool? ShowDialog()
        {
            switch (this.Dialog.ShowDialog())
            {
                case CommonFileDialogResult.None:
                    return null;
                case CommonFileDialogResult.Ok:
                    return true;
                case CommonFileDialogResult.Cancel:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool? ShowDialog(Window window)
        {
            switch (this.Dialog.ShowDialog(window))
            {
                case CommonFileDialogResult.None:
                    return null;
                case CommonFileDialogResult.Ok:
                    return true;
                case CommonFileDialogResult.Cancel:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}