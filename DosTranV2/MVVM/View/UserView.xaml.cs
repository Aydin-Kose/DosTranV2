using DosTranV2.MVVM.Model;
using DosTranV2.MVVM.ViewModel;
using System.Windows.Controls;

namespace DosTranV2.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        public UserModel model
        {
            get { return (UserModel)DataContext; }
        }
        public UserView()
        {
            InitializeComponent();
        }
    }
}
