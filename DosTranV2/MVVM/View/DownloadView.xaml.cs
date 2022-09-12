using DosTranV2.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace DosTranV2.MVVM.View
{
    /// <summary>
    /// Interaction logic for DownloadView.xaml
    /// </summary>
    public partial class DownloadView : UserControl
    {
        private DownloadViewModel model
        {
            get { return (DownloadViewModel)DataContext; }
        }
        public DownloadView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
