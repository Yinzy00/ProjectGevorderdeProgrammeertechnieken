using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace C_Our_Souls_WPF.ViewModels
{
    public abstract class BasisViewModel : IDataErrorInfo, INotifyPropertyChanged, ICommand
    {
        #region ICommand

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfo

        public abstract string this[string columnName] { get; }

        public string Error
        {
            get
            {
                string foutmeldingen = "";
                foreach (var item in this.GetType().GetProperties()) //reflection
                {
                    string fout = this[item.Name];
                    if (!string.IsNullOrWhiteSpace(fout))
                    {
                        foutmeldingen += fout + Environment.NewLine;
                    }
                }
                return foutmeldingen;
            }
        }

        #endregion

        #region hulpmethodes

        public bool IsGeldig()
        {
            return string.IsNullOrWhiteSpace(Error);
        }
        #endregion hulpmethodes
    }
}
