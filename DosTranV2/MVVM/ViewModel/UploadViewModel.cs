using DosTranV2.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosTranV2.MVVM.ViewModel
{
    internal class UploadViewModel: BaseViewModel
    {
		private string _fileLocation;

		public string FileLocation
        {
			get { return _fileLocation; }
			set { 
				_fileLocation = value;
				OnPropertyChanged();
			}
		}


		public UploadViewModel()
		{

		}
	}
}
