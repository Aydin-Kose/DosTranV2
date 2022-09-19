using DosTranV2.Core;
using DosTranV2.Data;
using DosTranV2.Data.Model;
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
        private ApplicationDbContext db;
        
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

        public UploadViewModel(ApplicationDbContext dbContext)
        {
            db = dbContext;
            FTPUpload = new Command(UploadAction);
        }

        private void UploadAction(object parameter)
        {
            if (Validate(((UploadView)parameter)))
            {
                var FTPClient = new FTPClient(UserModel.SelectedEnvironment.IP, UserModel.OpID, ((UploadView)parameter).UserComponent.passwordBox.Password);
                var result = FTPClient.Upload(DataSet, FileLocation);
                UIMessage = result;
                if (result == "İşlem Başarılı")
                {
                    db.DOSTRAN_LOG.Add(new DOSTRAN_LOG
                    {
                        DataSet = DataSet,
                        OpID = UserModel.OpID,
                        IslemTipi = "Upload",
                        TarihSaat = DateTime.Now
                    });
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Database Hata: \n" + ex.Message);
                    }
                    ((UploadView)parameter).messageBorder.BorderBrush = Brushes.DarkGreen;
                }
                else
                {
                    ((UploadView)parameter).messageBorder.BorderBrush = Brushes.DarkRed;
                }
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
                view.datasetBox.Focus();
                return false;
            }
            return true;
        }
    }
}
