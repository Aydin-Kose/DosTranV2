using DosTranV2.MVVM.Model;
using System.Collections.Generic;

namespace DosTranV2.MVVM.ViewModel
{
    internal class UploadViewModel
    {
        private string _opID;
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        public UserModel UserModel { get; set; }

        public UploadViewModel()
        {
        }
    }
}
