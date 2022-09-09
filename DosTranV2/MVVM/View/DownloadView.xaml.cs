using DosTranV2.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DosTranV2.MVVM.View
{
    /// <summary>
    /// Interaction logic for DownloadView.xaml
    /// </summary>
    public partial class DownloadView : UserControl
    {
        public DownloadViewModel model;
        public DownloadView()
        {
            model = (DownloadViewModel)DataContext;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
