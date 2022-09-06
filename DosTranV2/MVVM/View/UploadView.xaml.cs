using DosTranV2.MVVM.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            model = (UploadViewModel)this.DataContext;
        }

        private void File_Drop(object sender, DragEventArgs e)
        {
            
        }

        private void File_Click(object sender, RoutedEventArgs e)
        {
             OpenFileDialog openFileDialog = new OpenFileDialog() { Multiselect=false, Filter = "Text|*.txt" };
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                model.FileLocation = openFileDialog.FileName;
            }
        }
    }
}
