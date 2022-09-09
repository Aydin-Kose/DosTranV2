using DosTranV2.Core;
using DosTranV2.MVVM.Model;

namespace DosTranV2.MVVM.ViewModel
{
    public class UploadViewModel : BaseViewModel
    {
        private string _opID;
        private string _fileName;
        private string _dataSet;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
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

        public UploadViewModel()
        {
        }
    }
}
