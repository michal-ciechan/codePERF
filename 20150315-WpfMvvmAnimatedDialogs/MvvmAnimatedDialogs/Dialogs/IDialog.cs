using System;

namespace MvvmAnimatedDialogs.Dialogs
{
    public interface IDialog
    {
        event EventHandler<DialogEventArgs> Closed;
    
    }
}