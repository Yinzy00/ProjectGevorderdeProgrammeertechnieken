using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.BasisModels
{
    public abstract class Basisklasse : IDataErrorInfo, INotifyPropertyChanged
    {
        public abstract string this[string columnName] { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsGeldig()
        {
            return string.IsNullOrWhiteSpace(Error);
        }

        public string Error
        {
            get
            {
                string foutmeldingen = "";


                foreach (var item in this.GetType().GetProperties()) //reflection

                {
                    if (item.CanRead)
                    {
                        string fout = this[item.Name];
                        if (!string.IsNullOrWhiteSpace(fout))
                        {
                            foutmeldingen += fout + Environment.NewLine;
                        }
                    }
                }
                return foutmeldingen;
            }
        }
    }
}