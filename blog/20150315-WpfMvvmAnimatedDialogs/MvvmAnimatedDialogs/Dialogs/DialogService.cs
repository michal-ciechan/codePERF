using System;
using System.Windows;

namespace MvvmAnimatedDialogs.Dialogs
{
    public class DialogService : IDialogService
    {
        public IContinueWith<TDialog> ShowDialog<TDialog>(TDialog dialog) where TDialog : IDialog
        {
            WeakEventManager<TDialog, DialogEventArgs>.AddHandler(dialog, "Closed", (sender, args) =>
            {
                OnDialogClosed(args);
            });

            OnDialogShown(new DialogEventArgs(dialog));

            return new DialogHandler<TDialog>(dialog);
        }

        public event EventHandler<DialogEventArgs> DialogShown;
        public event EventHandler<DialogEventArgs> DialogClosed;



        protected virtual void OnDialogShown(DialogEventArgs e)
        {
            var handler = DialogShown;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDialogClosed(DialogEventArgs e)
        {
            var handler = DialogClosed;
            if (handler != null) handler(this, e);
        }
    }
}