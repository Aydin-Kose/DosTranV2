using DosTranV2.MVVM.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace DosTranV2.MVVM.View
{
    /// <summary>
    /// Interaction logic for UploadView.xaml
    /// </summary>
    public partial class UploadView : UserControl
    {
        private List<string> lastGeneratedDatasetList;

        private UploadViewModel model
        {
            get { return (UploadViewModel)DataContext; }
        }

        public UploadView()
        {
            InitializeComponent();
        }

        private void datasetBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var cbox = sender as System.Windows.Controls.ComboBox;
            lastGeneratedDatasetList = model.FillDataset();
            cbox.ItemsSource = lastGeneratedDatasetList;
        }

        private void datasetBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var cbox = sender as System.Windows.Controls.ComboBox;
            if (string.IsNullOrWhiteSpace(cbox.Text))
            {
                cbox.ItemsSource = lastGeneratedDatasetList;
            }
            else
            {
                cbox.ItemsSource = lastGeneratedDatasetList?.FindAll(x => x.Contains(cbox.Text.ToUpper()));
                if (((List<string>)cbox.ItemsSource).Count > 0 && e.Key != Key.Enter)
                {
                    cbox.IsDropDownOpen = true;
                }
                else
                {
                    cbox.IsDropDownOpen = false;
                }
            }
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
