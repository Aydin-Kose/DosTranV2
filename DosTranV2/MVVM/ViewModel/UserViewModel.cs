using DosTranV2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DosTranV2.MVVM.ViewModel
{
    internal class UserViewModel : BaseViewModel
    {
        private string _opID;
        private List<EnvironmentModel> _environmentList;
        private EnvironmentModel _selectedEnvironment;

        public string OpID
        {
            get { return _opID; }
            set { _opID = value; }
        }
        public List<EnvironmentModel> EnvironmentList
        {
            get { return _environmentList; }
            set
            {
                _environmentList = value;
                OnPropertyChanged(nameof(EnvironmentList));
            }
        }
        public EnvironmentModel SelectedEnvironment
        {
            get { return _selectedEnvironment; }
            set { _selectedEnvironment = value; }
        }

        public UserViewModel()
        {
            LoadModel();
        }

        private void LoadModel()
        {
            _opID = Environment.UserName;
            EnvironmentList = ApplicationConstant.EnvironmentList;
            SelectedEnvironment = EnvironmentList[1];
        }
    }

    
}
