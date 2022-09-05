using DosTranV2.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosTranV2.MVVM.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public RelayCommand UploadViewCommand { get; set; }
        public RelayCommand DownloadViewCommand { get; set; }
        UploadViewModel UploadVM { get; set; }
        DownloadViewModel DownloadVM { get; set; }
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            UploadVM = new UploadViewModel();
            DownloadVM = new DownloadViewModel();

            CurrentView = UploadVM;

            UploadViewCommand = new RelayCommand(o => { CurrentView = UploadVM; });
            DownloadViewCommand = new RelayCommand(o => { CurrentView = DownloadVM; });
        }
    }
}
