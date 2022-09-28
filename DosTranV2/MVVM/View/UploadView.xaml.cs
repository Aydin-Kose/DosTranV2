using DosTranV2.MVVM.ViewModel;
using Microsoft.Win32;
using System.IO;
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

        private void FileSelect_DragOver(object sender, DragEventArgs e)
        {
            bool dropEnabled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                if (filenames.Length != 1 || Path.GetExtension(filenames[0]).ToUpperInvariant() != ".TXT")
                {
                    dropEnabled = false;
                }
            }
            else
            {
                dropEnabled = false;
            }

            if (!dropEnabled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void FileSelect_Drop(object sender, DragEventArgs e)
        {
            string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            string fileLocation = filenames[0];
            model.FileName = Path.GetFileName(fileLocation);
            model.FileLocation = fileLocation;
        }
    }
}
