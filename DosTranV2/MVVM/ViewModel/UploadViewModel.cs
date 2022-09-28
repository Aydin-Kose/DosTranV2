﻿using DosTranV2.Core;
using DosTranV2.MVVM.Model;
using DosTranV2.MVVM.View;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace DosTranV2.MVVM.ViewModel
{
    public class UploadViewModel : BaseViewModel
    {
        private string _fileName;
        private string _dataSet;
        private string _uiMessage;
        
        public string FileLocation { get; set; }
        public string FileName
        {
            get { return _fileName; }
            set { 
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }
        public UserModel UserModel { get; set; }
        public string DataSet
        {
            get { return _dataSet; }
            set 
            { 
                _dataSet = value;
                OnPropertyChanged("DataSet");
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
        public Command FTPUpload { get; set; }

        public UploadViewModel()
        {
            FTPUpload = new Command(UploadAction);
        }

        private void UploadAction(object parameter)
        {
            if (Validate(((UploadView)parameter)))
            {
                var FTPClient = new FTPClient(UserModel.SelectedEnvironment.IP, UserModel.OpID, ((UserView)parameter).passwordBox.Password);
                FTPClient.Upload(DataSet, FileLocation);
            }
            else
            {
                ((UploadView)parameter).messageBorder.BorderBrush = Brushes.DarkRed;
            }
        }

        private bool Validate(UploadView view)
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
            else if (string.IsNullOrWhiteSpace(FileName))
            {
                view.fileSelectButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                return false;
            }
            else if (string.IsNullOrWhiteSpace(DataSet))
            {
                UIMessage = "DataSet alanı boş olamaz";
                view.UserComponent.passwordBox.Focus();
                return false;
            }
            return true;
        }
    }
}
