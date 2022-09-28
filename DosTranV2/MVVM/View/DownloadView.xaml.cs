using DosTranV2.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;

namespace DosTranV2.MVVM.View
{
    /// <summary>
    /// Interaction logic for DownloadView.xaml
    /// </summary>
    public partial class DownloadView : UserControl
    {
        private List<string> lastGeneratedDatasetList;

        private DownloadViewModel model
        {
            get { return (DownloadViewModel)DataContext; }
        }

        public DownloadView()
        {
            InitializeComponent();
        }

        private void FileLocationButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                model.FileLocation = folderBrowserDialog.SelectedPath;
            }
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
                if (((List<string>)cbox.ItemsSource).Count > 0)
                {
                    cbox.IsDropDownOpen = true;
                }
                else
                {
                    cbox.IsDropDownOpen = false;
                }
            }
        }
    }
}
