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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace C_Our_Souls_WPF.Components
{
    /// <summary>
    /// Interaction logic for DashboardComponent.xaml
    /// </summary>
    public partial class DashboardComponent : UserControl
    {
        public DashboardComponent()
        {
            InitializeComponent();
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(DashboardComponent), new PropertyMetadata(string.Empty));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Content", typeof(string), typeof(DashboardComponent), new PropertyMetadata(string.Empty));

        //public string Button
        //{
        //    get { return (string)GetValue(ButtonProperty); }
        //    set { SetValue(ButtonProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Button.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ButtonProperty =
        //    DependencyProperty.Register("Button", typeof(string), typeof(DashboardComponent), new PropertyMetadata(string.Empty));

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("De button werkt");
        //}
    }
}