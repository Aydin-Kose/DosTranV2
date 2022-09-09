using DosTranV2.MVVM.ViewModel;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace DosTranV2.MVVM.View
{
    /// <summary>
    /// Interaction logic for UploadView.xaml
    /// </summary>
    public partial class UploadView : UserControl
    {
        private UploadViewModel model
        {
            get { return (UploadViewModel)DataContext; }
        }


        public UploadView()
        {
            InitializeComponent();
        }

        private void FileSelect_Drop(object sender, DragEventArgs e)
        {

        }

        private void FileSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Multiselect = false, Filter = "Text|*.txt" };
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                model.FileLocation = openFileDialog.FileName;
                model.FileName = openFileDialog.SafeFileName;
            }
        }

    }
}
