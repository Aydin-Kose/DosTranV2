using DosTranV2.Core;
using DosTranV2.Data;
using DosTranV2.MVVM.Model;
using System;
using System.Linq;
using System.Windows;

namespace DosTranV2.MVVM.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private string _title;
        private BaseViewModel _currentView;
        private ApplicationDbContext dbContext;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        public Command UploadViewCommand { get; set; }
        public Command DownloadViewCommand { get; set; }
        public Command MoveWindowCommand { get; set; }
        public Command ShutdownWindowCommand { get; set; }
        public Command MinimizeWindowCommand { get; set; }
        UploadViewModel UploadVM { get; set; }
        DownloadViewModel DownloadVM { get; set; }
        UserModel UserModel { get; set; }
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
            LoadModel();
            UserModel = new UserModel();
            UploadVM = new UploadViewModel(dbContext) { UserModel = UserModel };
            DownloadVM = new DownloadViewModel(dbContext) { UserModel = UserModel };

            CurrentView = DownloadVM;

            SetCommands();
        }

        private void LoadModel()
        {
            dbContext = new ApplicationDbContext();
            if (!dbContext.Database.Exists())
            {
                var result = MessageBox.Show("Database bağlantısı kurulamadı uygulama kapatılacak.");
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
            var latestVersion = dbContext.DOSTRAN_VERSION.OrderByDescending(x => x.Id).FirstOrDefault();
            if (Properties.Settings.Default.Version != latestVersion.Version)
            {
                var result = MessageBox.Show("Lütfen uygulamanın son versiyonunu indiriniz." + latestVersion.Link);
                if (result == MessageBoxResult.OK)
                {
                    System.Diagnostics.Process.Start("http://" + latestVersion.Link);
                    Application.Current.Shutdown();
                }
            }
            Title = "DosTran Ver." + Properties.Settings.Default.Version;
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