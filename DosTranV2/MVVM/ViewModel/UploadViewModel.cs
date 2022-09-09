using DosTranV2.Core;
using DosTranV2.MVVM.Model;
using System;

namespace DosTranV2.MVVM.ViewModel
{
    public class UploadViewModel : BaseViewModel
    {
        private string _opID;
        private string _fileName;
        private string _dataSet;
        
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
        public Command FTPUpload { get; set; }

        public UploadViewModel()
        {
            FTPUpload = new Command(UploadAction);
        }

        private void UploadAction(object parameter)
        {
            var a = UserModel.Password;
        }
    }
}
