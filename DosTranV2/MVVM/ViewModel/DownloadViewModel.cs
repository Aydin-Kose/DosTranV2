using DosTranV2.Core;
using DosTranV2.Data;
using DosTranV2.Data.Model;
using DosTranV2.MVVM.Model;
using DosTranV2.MVVM.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace DosTranV2.MVVM.ViewModel
{
    public class DownloadViewModel : BaseViewModel
    {
        private string _fileLocation;
        private string _dataSet;
        private string _seperator;
        private string _uiMessage;
        private ApplicationDbContext db;

        public string FileLocation
        {
            get
            {
                return _fileLocation;
            }
            set
            {
                _fileLocation = value;
                OnPropertyChanged("FileLocation");
            }
        }
        public string FileType { get; set; }
        public string DataSet
        {
            get { return _dataSet; }
            set
            {
                _dataSet = value;
                OnPropertyChanged("DataSet");
            }
        }
        public string Seperator
        {
            get { return _seperator; }
            set
            {
                _seperator = value;
                OnPropertyChanged("Seperator");
            }
        }
        public string UIMessage
        {
            get { return _uiMessage; }
            set
            {
                _uiMessage = value;
                OnPropertyChanged("UIMessage");
            }
        }
        public UserModel UserModel { get; set; }
        public Command FTPDownload { get; set; }

        public DownloadViewModel(ApplicationDbContext dbContext)
        {
            db = dbContext;
            FileType = "Text";
            Seperator = ";";
            FileLocation = Path.GetTempPath();
            FTPDownload = new Command(DownloadAction);
        }

        private void DownloadAction(object parameter)
        {
            ((DownloadView)parameter).messageBorder.BorderBrush = Brushes.Gray;
            UIMessage = "İşlem Başladı";
            if (Validate(((DownloadView)parameter)))
            {
                var FTPClient = new FTPClient(UserModel.SelectedEnvironment.IP, UserModel.OpID, ((DownloadView)parameter).UserComponent.passwordBox.Password);
                var result = FTPClient.Download(DataSet, $"{FileLocation}\\{DataSet}.{(FileType == "Text" ? "txt" : "csv")}");
                UIMessage = result;
                if (result == "İşlem Başarılı")
                {
                    ((DownloadView)parameter).messageBorder.BorderBrush = Brushes.DarkGreen;
                    db.DOSTRAN_LOG.Add(new DOSTRAN_LOG
                    {
                        DataSet = DataSet,
                        OpID = UserModel.OpID,
                        IslemTipi = "Download",
                        TarihSaat = DateTime.Now
                    });
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Database Hata: \n" + ex.Message);
                    }
                }
                else
                {
                    ((DownloadView)parameter).messageBorder.BorderBrush = Brushes.DarkRed;
                }
            }
            else
            {
                ((DownloadView)parameter).messageBorder.BorderBrush = Brushes.DarkRed;
            }
        }

        private bool Validate(DownloadView view)
        {
            if (string.IsNullOrWhiteSpace(UserModel.OpID))
            {
                UIMessage = "Op ID alanı boş olamaz";
                view.UserComponent.opIdBox.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(view.UserComponent.passwordBox.Password))
            {
                UIMessage = "Şifre alanı boş olamaz";
                view.UserComponent.passwordBox.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(DataSet))
            {
                UIMessage = "DataSet alanı boş olamaz";
                view.datasetBox.Focus();
                return false;
            }
            else if (FileType == "Excel" && string.IsNullOrWhiteSpace(Seperator))
            {
                UIMessage = "Dosya tipi Excel ise seperatör alanı boş olamaz";
                view.seperatorBox.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(FileLocation))
            {
                UIMessage = "Dosya seçimi yapılmalıdır.";
                view.seperatorBox.Focus();
                return false;
            }
            return true;
        }
    }
}
