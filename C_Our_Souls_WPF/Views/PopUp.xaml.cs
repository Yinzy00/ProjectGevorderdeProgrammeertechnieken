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

namespace C_Our_Souls_WPF.Views
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        public new PopUpResponse DialogResult { get; set; }
        /// <summary>
        /// Popup with title and text 
        /// With Ok button
        /// </summary>
        /// <param name="_titel">Popup title as string</param>
        /// <param name="_text">Popup text as string</param>
        public PopUp(string _titel, string _text)
        {
            InitializeComponent();
            SetValues(_titel, _text, PopupButtonOptions.Ok);
        }
        /// <summary>
        /// Popup with tile and button options
        /// </summary>
        /// <param name="_titel">Popup title as string</param>
        /// <param name="buttonOptions">Popup type enum</param>
        public PopUp(string _titel, PopupButtonOptions buttonOptions)
        {
            InitializeComponent();
            SetValues(_titel, null, buttonOptions);
        }
        /// <summary>
        /// Popup with title, text and button options
        /// </summary>
        /// <param name="_titel">Popup title as string</param>
        /// <param name="_text">Popup text as string</param>
        /// <param name="buttonOptions">Popup type enum</param>
        public PopUp(string _titel, string _text ,PopupButtonOptions buttonOptions)
        {
            InitializeComponent();
            SetValues(_titel, _text, buttonOptions);
        }
        /// <summary>
        /// /// Popup with title, text and button options and custom button text
        /// </summary>
        /// <param name="_titel">Popup title as string</param>
        /// <param name="_text">Popup text as string</param>
        /// <param name="buttonOptions">Popup type enum</param>
        /// <param name="buttonText">Button text parameters: Ok => 1 parameter, OkCancel => 2 parameters ([0] == Ok button, [1] == Cancel button)</param>
        public PopUp(string _titel, string _text, PopupButtonOptions buttonOptions, params string[] buttonText)
        {
            InitializeComponent();
            SetValues(_titel, _text, buttonOptions, buttonText);
        }
        private void SetValues(string _titel, string _text, PopupButtonOptions buttonOptions, params string[] buttonText)
        {
            SetButtons(buttonOptions, buttonText);
            SetText(_titel, _text);
        }
        private void SetText(string _titel, string _text)
        {
            titel.Text = _titel;
            text.Text = _text;
        }
        private void SetButtons(PopupButtonOptions buttonOptions, params string []buttonText)
        {
            switch (buttonOptions)
            {
                case PopupButtonOptions.OkCancel:
                    ok.Visibility = Visibility.Visible;
                    cancel.Visibility = Visibility.Visible;

                    if (buttonText.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(buttonText[0]))
                        {
                            ok.Content = buttonText[0];
                        }
                        if (!string.IsNullOrEmpty(buttonText[1]))
                        {
                            cancel.Content = buttonText[1];
                        }
                    }
                    break;
                case PopupButtonOptions.Ok:
                    ok.Visibility = Visibility.Visible;
                    cancel.Visibility = Visibility.Hidden;
                    if (buttonText.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(buttonText[0]))
                        {
                            ok.Content = buttonText[0];
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public new void Show()
        {
            base.Show();
        }
        public enum PopupButtonOptions
        {
            OkCancel,
            Ok
        }
        public enum PopUpResponse
        {
            Closed,
            Ok,
            Cancel,
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = PopUpResponse.Ok;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = PopUpResponse.Cancel;
            this.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = PopUpResponse.Closed;
            this.Close();
        }
    }
}
