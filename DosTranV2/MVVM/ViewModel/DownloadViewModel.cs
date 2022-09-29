﻿using DosTranV2.Core;
using DosTranV2.DBModel.Model;
using DosTranV2.MVVM.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Media;
using System.Windows;
using DosTranV2.Data;
using System.IO;
using System.Net;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace DosTranV2.MVVM.ViewModel
{
    internal class DownloadViewModel : BaseViewModel
    {
        private ApplicationDbContext dbContext;
        private List<string> _dataSetList;
        private string _dataSet;
        private char? _seperator;
        private string _fileLocation;
        private MainViewModel mainViewModel;

        public List<string> DataSetList
        {
            get { return _dataSetList; }
            set
            {
                _dataSetList = value;
                OnPropertyChanged(nameof(DataSetList));
            }
        }
        public string DataSet
        {
            get { return _dataSet; }
            set
            {
                _dataSet = value;
                OnPropertyChanged(nameof(DataSet));
            }
        }
        public string FileType { get; set; }
        public char? Seperator
        {
            get { return _seperator; }
            set
            {
                _seperator = value;
                OnPropertyChanged(nameof(Seperator));
            }
        }
        public string FileLocation
        {
            get
            {
                return _fileLocation;
            }
            set
            {
                _fileLocation = value;
                OnPropertyChanged(nameof(FileLocation));
            }
        }

        public DownloadViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            dbContext = new ApplicationDbContext(ApplicationConstant.CONN_STRING);
            SetCommands();
        }

        #region Command
        public Command GetFileLocation { get; set; }
        public Command FTPDownload { get; set; }
        private void SetCommands()
        {
            GetFileLocation = new Command(GetFileLocationAction);
            FTPDownload = new Command(DownloadAction);
            dbContext = new ApplicationDbContext(ApplicationConstant.CONN_STRING);
            FileType = "Text";
            Seperator = ';';
            FileLocation = Path.GetTempPath();
        }

        private void GetFileLocationAction(object obj)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                FileLocation = folderBrowserDialog.SelectedPath;
            }
        }

        private void DownloadAction(object parameter)
        {
            var mainWindow = (MainWindow)parameter;

            if (Validate(mainWindow))
            {
                mainViewModel.BorderColor = (Brush)System.Windows.Application.Current.FindResource("NormalBorderBrush");
                mainViewModel.UIMessage = "İşlem başladı.";
                var FTPClient = new FTPClient(mainViewModel.UserVM.SelectedEnvironment.IP, mainViewModel.UserVM.OpID, mainWindow.UserComponent.passwordBox.Password);
                var result = FTPClient.Download(DataSet, $"{FileLocation}\\{DataSet}.{(FileType == "Text" ? "txt" : "xls")}", FileType, Seperator);
                mainViewModel.UIMessage = result;
                if (result == "İşlem Başarılı")
                {
                    mainViewModel.BorderColor = (Brush)System.Windows.Application.Current.FindResource("SuccessBorderBrush");
                    LogDownload();
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

        internal List<string> FillDataset()
        {
            try
            {
                return dbContext.DOSTRAN_DOWNLOAD.Where(x => x.OpID == mainViewModel.UserVM.OpID).Select(x => x.DataSet).ToList();
            }
            catch (Exception ex)
            {
                mainViewModel.UIMessage = "Dataset bilgisi alınamadı: " + ex.Message;
                mainViewModel.BorderColor = (Brush)System.Windows.Application.Current.FindResource("AlertBorderBrush");
                return new List<string>();
            }
        }

        private void LogDownload()
        {
            dbContext.DOSTRAN_LOG.Add(new DOSTRAN_LOG
            {
                DataSet = DataSet,
                OpID = mainViewModel.UserVM.OpID,
                UserName = Environment.UserName,
                IP = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString(),
                IslemTipi = "Download",
                TarihSaat = DateTime.Now
            });
            if (!FillDataset().Any(x => x == DataSet))
            {
                dbContext.DOSTRAN_DOWNLOAD.Add(new DOSTRAN_DOWNLOAD
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

        private bool Validate(MainWindow view)
        {
            var downloadview = (DownloadView)VisualTreeHelper.GetChild(view.content, 0);
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
            else if (string.IsNullOrWhiteSpace(DataSet))
            {
                mainViewModel.UIMessage = "DataSet alanı boş olamaz";
                downloadview.datasetBox.Focus();
                return false;
            }
            else if (FileType == "Excel" && Seperator!=null)
            {
                mainViewModel.UIMessage = "Dosya tipi Excel ise seperatör alanı boş olamaz";
                downloadview.seperatorBox.Focus();
                return false;
            }
            return true;
        }
        #endregion

    }
}
