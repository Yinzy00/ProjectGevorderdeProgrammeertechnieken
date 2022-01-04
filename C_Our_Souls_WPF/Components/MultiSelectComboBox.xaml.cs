using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox : UserControl
    {
        private ObservableCollection<Node> _nodeList = new ObservableCollection<Node>();

        public MultiSelectComboBox()
        {
            InitializeComponent();
            _nodeList = new ObservableCollection<Node>();
        }

        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty =
             DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<object>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(MultiSelectComboBox.OnItemsSourceChanged)));

        public static readonly DependencyProperty SelectedItemsProperty =
         DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
     new PropertyChangedCallback(MultiSelectComboBox.OnSelectedItemsChanged)));

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public string DisplayMemberPath
        {
            get
            {
                return (string)GetValue(DisplayMemberPathProperty);
            }
            set
            {
                SetValue(DisplayMemberPathProperty, value);
            }
        }

        public ObservableCollection<object> ItemsSource
        {
            get { return (ObservableCollection<object>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public ObservableCollection<object> SelectedItems
        {
            get { return (ObservableCollection<object>)GetValue(SelectedItemsProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        #endregion Dependency Properties

        #region Events

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.DisplayInControl();
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.SelectNodes();
            control.SetText();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox clickedBox = (CheckBox)sender;

            if (clickedBox.Content == "All")
            {
                foreach (Node node in _nodeList)
                {
                    node.IsSelected = true;
                }
            }
            else
            {
                int _selectedCount = 0;
                foreach (Node s in _nodeList)
                {
                    if (s.IsSelected && s.Title != "All")
                        _selectedCount++;
                }
                if (_selectedCount == _nodeList.Count - 1)
                    _nodeList.FirstOrDefault(i => i.Title == "All").IsSelected = true;
                else
                    _nodeList.FirstOrDefault(i => i.Title == "All").IsSelected = false;
            }
            SetSelectedItems();
            SetText();
        }

        #endregion Events

        #region Methods

        public void SelectNodes()
        {
            if (SelectedItems.Count == 0)
            {
                foreach (var node in _nodeList)
                {
                    if (node != null)
                        node.IsSelected = false;
                }
            }
            else
            {
                foreach (object _object in SelectedItems)
                {
                    Node node = _nodeList.FirstOrDefault(i => i.Object == _object);
                    if (node != null)
                        node.IsSelected = true;
                }
            }
            
        }

        private void SetSelectedItems()
        {
            //if (ItemsSource.Count != 0)
            //{
                if (SelectedItems == null)
                    SelectedItems = new ObservableCollection<object>();
                SelectedItems.Clear();
                foreach (Node node in _nodeList)
                {
                    if (node.IsSelected && node.Title != "All")
                    {
                        if (this.ItemsSource.Count > 0)
                        {
                            SelectedItems.Add(node.Object);
                        }
                    }
                }
            //}
            //else
            //{
            //    foreach (Node node in _nodeList)
            //    {
            //        node.IsSelected = false;
            //    }
            //}
        }

        private void DisplayInControl()
        {
            _nodeList.Clear();
            if (this.ItemsSource.Count > 0)
                _nodeList.Add(new Node("All", null));
            foreach (object _object in this.ItemsSource)
            {
                var type = _object.GetType();
                string value = type.GetProperty(DisplayMemberPath).GetValue(_object, null).ToString();
                Node node = new Node(value, _object);
                _nodeList.Add(node);
            }
            MultiSelectCombo.ItemsSource = _nodeList;
        }

        private void SetText()
        {
            if (this.SelectedItems != null)
            {
                StringBuilder displayText = new StringBuilder();
                foreach (Node s in _nodeList)
                {
                    if (s.IsSelected == true && s.Title == "All")
                    {
                        displayText = new StringBuilder();
                        displayText.Append("All");
                        break;
                    }
                    else if (s.IsSelected == true && s.Title != "All")
                    {
                        displayText.Append(s.Title);
                        displayText.Append(',');
                    }
                }
                this.Text = displayText.ToString().TrimEnd(new char[] { ',' });
            }
            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(this.Text))
            {
                this.Text = this.DefaultText;
            }
        }

        #endregion Methods
    }

    public class Node : INotifyPropertyChanged
    {
        private string _title;
        private object _object;
        private bool _isSelected;

        #region ctor

        public Node(string title, object __object)
        {
            Title = title;
            Object = __object;
        }

        #endregion ctor

        public object Object
        {
            get => _object; set
            {
                _object = value;
                NotifyPropertyChanged("Object");
            }
        }

        #region Properties

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        #endregion Properties

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        

    }
}