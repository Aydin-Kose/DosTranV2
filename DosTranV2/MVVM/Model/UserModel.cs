using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DosTranV2.MVVM.Model
{
    internal class UserModel
    {
        private string _opID;
        private List<EnvironmentModel> _environmentList;
        private string selectedEnvironmentIP;

        public string OpID
		{
			get { return _opID; }
			set { _opID = value; }
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

        public UserModel()
        {
            LoadModel();
        }

        private void LoadModel()
        {
            EnvironmentList = new List<EnvironmentModel> {
                new EnvironmentModel { Name="Dev", IP= Properties.Settings.Default.DevIP},
                new EnvironmentModel { Name="Test", IP= Properties.Settings.Default.TestIP},
                new EnvironmentModel { Name="Prod", IP= Properties.Settings.Default.ProdIP}
            };
        }
    }
}
