using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace MvvmAnimatedDialogs.Dialogs
{
    public class SimpleDialog : ViewModelBase, IDialog
    {
        private RelayCommand _commandOk;
        private string _message;

        public SimpleDialog(string message)
        {
            _message = message;

            CommandOk = new RelayCommand(OnClosed);
        }

        public RelayCommand CommandOk
        {
            get { return _commandOk; }
            set
            {
                if (_commandOk == value) return;
                _commandOk = value;
                RaisePropertyChanged(() => CommandOk);
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message == value) return;
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public event EventHandler<DialogEventArgs> Closed;

        public void OnClosed()
        {
            var closed = Closed;
            if (closed != null) closed(this, new DialogEventArgs(this));
        }
    }
}