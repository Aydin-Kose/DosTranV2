using DosTranV2.Core;
using DosTranV2.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security;
using System.Text;
using System.Windows.Controls;

namespace DosTranV2.MVVM.ViewModel
{
    internal class UploadViewModel : BaseViewModel
    {
        private string _fileLocation;
        private List<EnvironmentModel> _environmentList;
        private string selectedEnvironmentIP;
        private string _opID;
        private string _dataSet;


        public RelayCommand Upload { get; set; }
        public string FileLocation
        {
            get { return _fileLocation; }
            set
            {
                _fileLocation = value;
                OnPropertyChanged();
            }
        }
        public List<EnvironmentModel> EnvironmentList
        {
            get { return _environmentList; }
            set { _environmentList = value; }
        }
        public string SelectedEnvironmentIP
        {
            get { return selectedEnvironmentIP; }
            set { selectedEnvironmentIP = value; }
        }
        public string OpID
        {
            get { return _opID; }
            set { _opID = value; }
        }
        public string Password { private get; set; }
        public string DataSet
        {
            get { return _dataSet; }
            set { _dataSet = value; }
        }

        public UploadViewModel()
        {
            LoadModel();
            Upload = new RelayCommand(UploadAction);
        }

        private void LoadModel()
        {
            EnvironmentList = new List<EnvironmentModel> {
                new EnvironmentModel { Name="Dev", IP= Properties.Settings.Default.DevIP},
                new EnvironmentModel { Name="Test", IP= Properties.Settings.Default.TestIP},
                new EnvironmentModel { Name="Prod", IP= Properties.Settings.Default.ProdIP}
            };
        }

        private void UploadAction(object parameter)
        {
            FTP ftp = new FTP(SelectedEnvironmentIP, OpID, (parameter as PasswordBox).Password);
            ftp.Upload(DataSet, FileLocation);
        }
    }
}
