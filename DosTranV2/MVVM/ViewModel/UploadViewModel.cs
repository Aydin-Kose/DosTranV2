using DosTranV2.Core;
using DosTranV2.Data;
using DosTranV2.DBModel.Model;
using DosTranV2.MVVM.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DosTranV2.MVVM.ViewModel
{
    internal class UploadViewModel : BaseViewModel
    {
        private ApplicationDbContext dbContext;
        private List<string> _dataSetList;
        private string _dataSet;
        private MainViewModel mainViewModel;
        private string _fileName;

        public string DataSet
        {
            get { return _dataSet; }
            set
            {
                _dataSet = value;
                OnPropertyChanged(nameof(DataSet));
            }
        }

        public string FileLocation { get; set; }
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }
        public Command FTPUpload { get; set; }

        public UploadViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            dbContext = new ApplicationDbContext(ApplicationConstant.CONN_STRING);
            FTPUpload = new Command(UploadAction);
        }

        private void UploadAction(object parameter)
        {
            var mainWindow = (MainWindow)parameter;

            if (Validate(mainWindow))
            {
                mainViewModel.BorderColor = (Brush)System.Windows.Application.Current.FindResource("NormalBorderBrush");
                mainViewModel.UIMessage = "İşlem başladı.";
                var FTPClient = new FTPClient(mainViewModel.UserVM.SelectedEnvironment.IP, mainViewModel.UserVM.OpID, mainWindow.UserComponent.passwordBox.Password);
                var result = FTPClient.Upload(DataSet, FileLocation);
                mainViewModel.UIMessage = result;
                if (result == "İşlem Başarılı")
                {
                    mainViewModel.BorderColor = (Brush)System.Windows.Application.Current.FindResource("SuccessBorderBrush");
                    LogUpload();
                }
                else
                {
                    mainViewModel.BorderColor = (Brush)System.Windows.Application.Current.FindResource("AlertBorderBrush");
                }
            }
            else
            {
                mainViewModel.BorderColor = (Brush)System.Windows.Application.Current.FindResource("AlertBorderBrush");
            }
        }

        private bool Validate(MainWindow view)
        {
            var uploadView = (UploadView)VisualTreeHelper.GetChild(view.content, 0);
            if (string.IsNullOrWhiteSpace(mainViewModel.UserVM.OpID))
            {
                mainViewModel.UIMessage = "Op ID alanı boş olamaz";
                view.UserComponent.opIdBox.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(view.UserComponent.passwordBox.Password))
            {
                mainViewModel.UIMessage = "Şifre alanı boş olamaz";
                view.UserComponent.passwordBox.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(FileName))
            {
                uploadView.fileSelectButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                return false;
            }
            else if (string.IsNullOrWhiteSpace(DataSet))
            {
                mainViewModel.UIMessage = "DataSet alanı boş olamaz";
                uploadView.datasetBox.Focus();
                return false;
            }
            return true;
        }

        private void LogUpload()
        {
            dbContext.DOSTRAN_LOG.Add(new DOSTRAN_LOG
            {
                DataSet = DataSet,
                OpID = mainViewModel.UserVM.OpID,
                UserName = Environment.UserName,
                IP = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString(),
                IslemTipi = "Upload",
                TarihSaat = DateTime.Now
            });
            if (!FillDataset().Any(x => x == DataSet))
            {
                dbContext.DOSTRAN_UPLOAD.Add(new DOSTRAN_UPLOAD
                {
                    DataSet = DataSet,
                    OpID = mainViewModel.UserVM.OpID
                });
            }
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                mainViewModel.UIMessage = "Database Hata: \n" + ex.Message;
                mainViewModel.BorderColor = (Brush)System.Windows.Application.Current.FindResource("AlertBorderBrush");
            }
        }

        internal List<string> FillDataset()
        {
            try
            {
                return dbContext.DOSTRAN_UPLOAD.Where(x => x.OpID == mainViewModel.UserVM.OpID).Select(x => x.DataSet).ToList();
            }
            catch (Exception ex)
            {
                mainViewModel.UIMessage = "Dataset bilgisi alınamadı: " + ex.Message;
                mainViewModel.BorderColor = (Brush)System.Windows.Application.Current.FindResource("AlertBorderBrush");
                return new List<string>();
            }
        }
    }
}
