using DosTranV2.Core;
using System.Collections.Generic;

namespace DosTranV2.MVVM.Model
{
    public class UserModel : BaseViewModel
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
                OnPropertyChanged("EnvironmentList");
            }
        }
        public EnvironmentModel SelectedEnvironment
        {
            get { return _selectedEnvironment; }
            set { _selectedEnvironment = value; }
        }

        public UserModel()
        {
            LoadModel();
        }

        private void LoadModel()
        {
            _opID = System.Environment.MachineName;
            EnvironmentList = new List<EnvironmentModel> {
                new EnvironmentModel { Name="Dev", IP= Properties.Settings.Default.DevIP},
                new EnvironmentModel { Name="Test", IP= Properties.Settings.Default.TestIP},
                new EnvironmentModel { Name="Prod", IP= Properties.Settings.Default.ProdIP}
            };
            SelectedEnvironment = EnvironmentList[1];
        }
    }
}
