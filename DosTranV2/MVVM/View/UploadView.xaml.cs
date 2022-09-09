using DosTranV2.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DosTranV2.MVVM.View
{
    /// <summary>
    /// Interaction logic for UploadView.xaml
    /// </summary>
    public partial class UploadView : UserControl
    {
        UploadViewModel model;
        public UploadView()
        {
            InitializeComponent();
            model = (UploadViewModel)DataContext;
        }

        private void File_Drop(object sender, DragEventArgs e)
        {

        }

        private void File_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            model.DataSet = "a";
        }
    }
}
