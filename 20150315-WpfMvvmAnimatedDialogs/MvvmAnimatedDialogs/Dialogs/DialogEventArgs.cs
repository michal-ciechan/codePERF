using System;

namespace MvvmAnimatedDialogs.Dialogs
{
    public class DialogEventArgs : EventArgs
    {
        public DialogEventArgs(IDialog dialog)
        {
            Dialog = dialog;
        }

        public IDialog Dialog { get; private set; }
    }
}