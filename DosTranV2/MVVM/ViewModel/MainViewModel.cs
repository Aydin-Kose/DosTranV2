using DosTranV2.Core;
using DosTranV2.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace DosTranV2.MVVM.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private ApplicationDbContext dbContext;
        private string _uiMessage;
        private Brush _borderColor;

        public string UIMessage
        {
            get { return _uiMessage; }
            set
            {
                _uiMessage = value;
                OnPropertyChanged(nameof(UIMessage));
            }
        }
        public Brush BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        #region ViewModels
        private BaseViewModel _currentView;
        UploadViewModel UploadVM { get; set; }
        DownloadViewModel DownloadVM { get; set; }
        public UserViewModel UserVM { get; set; }
        public BaseViewModel CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        #endregion

        public MainViewModel()
        {
            dbContext = new ApplicationDbContext(ApplicationConstant.CONN_STRING);
            UserVM = new UserViewModel();
            UploadVM = new UploadViewModel(this);
            DownloadVM = new DownloadViewModel(this);

            CurrentView = DownloadVM;

            SetCommands();
        }

        #region Commands
        public Command UploadViewCommand { get; set; }
        public Command DownloadViewCommand { get; set; }
        public Command MoveWindowCommand { get; set; }
        public Command ShutdownWindowCommand { get; set; }
        public Command MinimizeWindowCommand { get; set; }
        public Command ContentRenderedCommand { get; set; }

        private void SetCommands()
        {
            MoveWindowCommand = new Command(o => { Application.Current.MainWindow.DragMove(); });
            ShutdownWindowCommand = new Command(o => { Application.Current.Shutdown(); });
            MinimizeWindowCommand = new Command(o => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });
            UploadViewCommand = new Command(o => { CurrentView = UploadVM; ClearUIMessage(); });
            DownloadViewCommand = new Command(o => { CurrentView = DownloadVM; ClearUIMessage(); });
            ContentRenderedCommand = new Command(ContentRendered);
        }
        #endregion
        public void ClearUIMessage()
        {
            BorderColor = (Brush)Application.Current.FindResource("NormalBorderBrush");
            UIMessage = "";
        }
        private void ContentRendered(object obj)
        {
            BorderColor = (Brush)Application.Current.FindResource("NormalBorderBrush");
            UIMessage = "Database Bağlantısı kuruluyor. Lütfen Bekleyin.";
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      if (!dbContext.Database.Exists())
                      {
                          MessageBox.Show("Database bağlantısı kurulamadı. Uygulama kapatılacak!");
                          Environment.Exit(0);
                      }
                  }));

            CheckVersion();
        }

        private void CheckVersion()
        {
            UIMessage = "Versiyon kontrolü yapılıyor";

            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      try
                      {
                          var latestVersion = dbContext.DOSTRAN_VERSION.OrderByDescending(x => x.Id).FirstOrDefault();
                          if (ApplicationConstant.VER != latestVersion.Version)
                          {
                              var result = MessageBox.Show("Lütfen uygulamanın son versiyonunu indiriniz." + latestVersion.Link);
                              if (result == MessageBoxResult.OK)
                              {
                                  System.Diagnostics.Process.Start("http://" + latestVersion.Link);
                                  Application.Current.Shutdown();
                              }
                          }
                          else
                          {
                              UIMessage = "Versiyon kontrolü başarılı";
                          }
                      }
                      catch (Exception)
                      {
                          MessageBox.Show("Versiyon bilgisi alınamadı. Uygulama kapatılacak!'");
                          Environment.Exit(0);
                      }
                  }));
        }
    }
}
