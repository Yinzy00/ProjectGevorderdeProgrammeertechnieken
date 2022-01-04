using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace C_Our_Souls_WPF.Views
{
    /// <summary>
    /// Interaction logic for AccountPopUpView.xaml
    /// </summary>
    public partial class AccountPopUpView : Window
    {
        public AccountPopUpView()
        {
            InitializeComponent();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width;
            this.Top = 60;
        }
    }
}