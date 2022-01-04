using C_Our_Souls_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace C_Our_Souls_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DashboardGebruikerView vm = new DashboardGebruikerView();
            vm.Show();
        }

        private void btnBeheer_Click(object sender, RoutedEventArgs e)
        {
            DashboardBeheerderView vm = new DashboardBeheerderView();
            vm.Show();
        }
    }
}