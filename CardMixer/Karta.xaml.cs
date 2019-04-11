using System;
using System.Collections.Generic;
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

namespace CardMixer
{
    /// <summary>
    /// Interakční logika pro Karta.xaml
    /// </summary>
    public partial class Karta : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int Cislo = 0;
        int _PocetK = 0;
        public int PocetK
        {
            get { return _PocetK; }
            set
            {
                _PocetK = value;
                OnPropertyChanged("PocetK");
            }
        }
        int _PocetO = 0;
        public int PocetO
        {
            get { return _PocetO; }
            set
            {
                _PocetO = value;
                OnPropertyChanged("PocetO");
            }
        }
        BitmapImage _Obrazek = null;
        public BitmapImage Obrazek
        {
            get { return _Obrazek; }
            set { _Obrazek = value; OnPropertyChanged("Obrazek"); }
        }
        string _Jmeno = "Jmeno";
        public string Jmeno
        {
            get { return _Jmeno; }
            set
            {
                _Jmeno = value;
                OnPropertyChanged("Jmeno");
            }
        }
        public Karta()
        {
            InitializeComponent();

            Binding b = new Binding("PocetO");
            b.Mode = BindingMode.OneWay;
            b.Source = this;
            pocetO.SetBinding(Label.ContentProperty, b);

            b = new Binding("PocetK");
            b.Mode = BindingMode.OneWay;
            b.Source = this;
            pocetK.SetBinding(Label.ContentProperty, b);

            b = new Binding("Obrazek");
            b.Mode = BindingMode.OneWay;
            b.Source = this;
            image.SetBinding(Image.SourceProperty, b);

            b = new Binding("Jmeno");
            b.Mode = BindingMode.OneWay;
            b.Source = this;
            jmeno.SetBinding(Label.ContentProperty, b);
        }
    }
}