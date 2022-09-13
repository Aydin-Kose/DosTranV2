using DosTranV2.Core;
using DosTranV2.MVVM.Model;
using DosTranV2.MVVM.View;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace DosTranV2.MVVM.ViewModel
{
    public class DownloadViewModel : BaseViewModel
    {
        private string _fileLocation;
        private string _dataSet;
        private string _seperator;
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
        public UserModel UserModel { get; set; }
        public Command FTPDownload { get; set; }

        public DownloadViewModel()
        {
            FileType = "Text";
            Seperator = ";";
            FileLocation = Path.GetTempPath();
            FTPDownload = new Command(DownloadAction);
        }

        private void DownloadAction(object parameter)
        {
            var FTPClient = new FTPClient(UserModel.SelectedEnvironment.IP, UserModel.OpID, ((UserView)parameter).pwBox.Password);
            FTPClient.Download(DataSet, $"{FileLocation}\\{DataSet}.{(FileType == "Text" ? "txt" : "csv")}");
        }
    }
}
