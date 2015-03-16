using System;
using System.Windows;

namespace MvvmAnimatedDialogs.Dialogs
{
    public class DialogHandler<TDialog> : IContinueWith<TDialog>
    {
        private Action<TDialog> _continueWith;
        private readonly TDialog _dialog;

        public DialogHandler(TDialog dialog)
        {
            _dialog = dialog;
        }

        public void ContinueWith(Action<TDialog> action)
        {
            _continueWith = action;
            WeakEventManager<TDialog, DialogEventArgs>.AddHandler(_dialog, "Closed", (sender, args) =>
            {
                _continueWith(_dialog);
            });
        }
    }
}