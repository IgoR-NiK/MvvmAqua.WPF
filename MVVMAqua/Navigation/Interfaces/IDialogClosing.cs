using System;

namespace MVVMAqua.Navigation.Interfaces
{
    public interface IDialogClosing
    {
        event CloseDialogEventHandler CloseDialog;
    }

    public delegate void CloseDialogEventHandler(object sender, CloseDialogEventArgs e);

    public class CloseDialogEventArgs : EventArgs
    {
        public bool DialogResult { get; }

        public CloseDialogEventArgs(bool dialogResult)
        {
            DialogResult = dialogResult;
        }
    }
}