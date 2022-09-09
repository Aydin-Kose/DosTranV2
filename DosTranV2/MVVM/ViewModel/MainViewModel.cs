using DosTranV2.Core;
using DosTranV2.MVVM.Model;
using System.Windows;

namespace DosTranV2.MVVM.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        public Command UploadViewCommand { get; set; }
        public Command DownloadViewCommand { get; set; }
        public Command MoveWindowCommand { get; set; }
        public Command ShutdownWindowCommand { get; set; }
        public Command MinimizeWindowCommand { get; set; }
        UploadViewModel UploadVM { get; set; }
        DownloadViewModel DownloadVM { get; set; }
        UserModel UserModel { get; set; }
        private BaseViewModel _currentView;
        public BaseViewModel CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }
        public MainViewModel()
        {
            UserModel = new UserModel();
            UploadVM = new UploadViewModel() { UserModel = UserModel };
            DownloadVM = new DownloadViewModel() { UserModel = UserModel };

            CurrentView = DownloadVM;

            SetCommands();
        }

        private void SetCommands()
        {
            MoveWindowCommand = new Command(o => { Application.Current.MainWindow.DragMove(); });
            ShutdownWindowCommand = new Command(o => { Application.Current.Shutdown(); });
            MinimizeWindowCommand = new Command(o => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });
            UploadViewCommand = new Command(o => { CurrentView = UploadVM; });
            DownloadViewCommand = new Command(o => { CurrentView = DownloadVM; });
        }
    }
}