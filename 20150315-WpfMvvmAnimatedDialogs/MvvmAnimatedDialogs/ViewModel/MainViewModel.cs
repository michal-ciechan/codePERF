#region

using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmAnimatedDialogs.Dialogs;

#endregion

namespace MvvmAnimatedDialogs.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private RelayCommand _commandAboutMe;
        private IDialog _currentDialog;
        private ObservableCollection<PostViewModel> _posts;

        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _dialogService.DialogShown += (sender, args) => CurrentDialog = args.Dialog;
            _dialogService.DialogClosed += (sender, args) => CurrentDialog = null;

            CommandAboutMe = new RelayCommand(() => _dialogService.ShowDialog(new SimpleDialog("If you want to know more about me...\r\n\r\nLeave a comment   :-D")));

            Posts = new ObservableCollection<PostViewModel>
            {
                new PostViewModel(dialogService)
                {
                    Title = ".NET Multiple LINQs vs Single foreach",
                    Summary =
                        "Following my work I did regarding improving performance by reducing the time taken from O(n²) to O(n) " +
                        "by introducing a Hashed data structure as per post .NET Nested Loops vs Hash Lookups, " +
                        "I was curious to see how much of an impact did using multiple LINQs have on performance " +
                        "in terms of time as compared to a single foreach…"
                },
                new PostViewModel(dialogService)
                {
                    Title = "Rx Request Response Throttle",
                    Summary =
                        "My most recent project at work was to introduce a Request Response messaging API into one our systems, " +
                        "as previously we were receiving a daily batch file with upwards of 4m entries when we only needed around 500-1,000 of them…"
                },
                new PostViewModel(dialogService)
                {
                    Title = ".NET Nested Loops vs Hash Lookups",
                    Summary =
                        "Recently I was tasked with improving the performance for a number of processes we have at work as " +
                        "some of these processes where taking as long as 25 mins to complete. After some initial investigation " +
                        "I noticed all 3 processes suffered from the same issue… Nested Loops."
                },
                new PostViewModel(dialogService)
                {
                    Title = "About Michal Ciechan.",
                    Summary =
                        "UK/London based .NET Full Stack DevOps Developer always looking for new challenges with a passion for all things tech, " +
                        "IT, finance, maths, and software related as well as everything else that comes along with it."
                }
            };
        }

        public RelayCommand CommandAboutMe
        {
            get { return _commandAboutMe; }
            set
            {
                if (_commandAboutMe == value) return;
                _commandAboutMe = value;
                RaisePropertyChanged(() => CommandAboutMe);
            }
        }

        public IDialog CurrentDialog
        {
            get { return _currentDialog; }
            set
            {
                if (_currentDialog == value) return;
                _currentDialog = value;
                RaisePropertyChanged(() => CurrentDialog);
            }
        }

        public ObservableCollection<PostViewModel> Posts
        {
            get { return _posts; }
            set
            {
                if (_posts == value) return;
                _posts = value;
                RaisePropertyChanged(() => Posts);
            }
        }
    }
}