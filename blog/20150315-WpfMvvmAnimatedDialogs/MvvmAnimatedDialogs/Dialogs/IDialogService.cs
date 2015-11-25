using System;

namespace MvvmAnimatedDialogs.Dialogs
{
    public interface IDialogService
    {
        IContinueWith<TDialog> ShowDialog<TDialog>(TDialog dialog) where TDialog : IDialog;
        
        event EventHandler<DialogEventArgs> DialogShown;
        event EventHandler<DialogEventArgs> DialogClosed;
    }

    public interface IContinueWith<out TType>
    {
        void ContinueWith(Action<TType> action);
    }
}
