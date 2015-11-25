using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace MvvmAnimatedDialogs.Dialogs
{
    public class OkCancelDialog : ViewModelBase, IDialog
    {
        private RelayCommand _commandCancel;
        private RelayCommand _commandOk;
        private string _message;

        public OkCancelDialog(string message)
        {
            _message = message;

            CommandOk = new RelayCommand(OnClosed);
            CommandCancel = new RelayCommand(OnClosed);
        }

        public RelayCommand CommandCancel
        {
            get { return _commandCancel; }
            set
            {
                if (_commandCancel == value) return;
                _commandCancel = value;
                RaisePropertyChanged(() => CommandCancel);
            }
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