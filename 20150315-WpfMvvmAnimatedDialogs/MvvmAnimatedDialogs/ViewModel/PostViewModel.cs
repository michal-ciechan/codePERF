using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmAnimatedDialogs.Dialogs;

namespace MvvmAnimatedDialogs.ViewModel
{
    public class PostViewModel : ViewModelBase
    {
        private RelayCommand _showPostCommand;
        private string _summary;
        private string _title;
        private Action _execute;

        public PostViewModel(IDialogService dialogService)
        {
            _execute = () =>
            {
                dialogService.ShowDialog(new OkCancelDialog("You are about to open a URL.\r\n\r\n" +
                                                            "Are you sure?"));
            };
            ShowPostCommand = new RelayCommand(_execute);
        }

        public RelayCommand ShowPostCommand
        {
            get { return _showPostCommand; }
            set
            {
                if (_showPostCommand == value) return;
                _showPostCommand = value;
                RaisePropertyChanged(() => ShowPostCommand);
            }
        }

        public string Summary
        {
            get { return _summary; }
            set
            {
                if (_summary == value) return;
                _summary = value;
                RaisePropertyChanged(() => Summary);
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }
    }
}