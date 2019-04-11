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
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace CardMixer
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        Timer timer;
        Timer timerK;
        List<List<string>> Karty = new List<List<string>>();
        List<List<string>> KartyOrg = new List<List<string>>();
        Random r = new Random();
        BitmapImage _Obrazek = null;
        public BitmapImage Obrazek
        {
            get { return _Obrazek; }
            set { _Obrazek = value; OnPropertyChanged("Obrazek"); }
        }
        BitmapImage ObrazekNext = null;
        public MainWindow()
        {
            InitializeComponent();
            
            //image
            Binding bin = new Binding("Obrazek");
            bin.Mode = BindingMode.OneWay;
            bin.Source = this;
            image.SetBinding(Image.SourceProperty, bin);

            timerK = new Timer(tickK);

            try
            {
                //deck load
                var deck = Directory.GetDirectories("Karty");
                for (int i = 0; i < deck.Length; i++)
                {
                    var k = new Karta();
                    k.Cislo = i;
                    k.Jmeno = Path.GetFileNameWithoutExtension(deck[i]);
                    var seznam = new List<string>();
                    seznam.AddRange(Directory.GetFiles(deck[i]));
                    k.Obrazek = new BitmapImage(new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), seznam[0]));
                    seznam.RemoveAt(0);
                    Karty.Add(seznam.ToList());
                    KartyOrg.Add(seznam.ToList());
                    k.reset.Click += delegate (object sender, RoutedEventArgs e)
                    {
                        var res = MessageBox.Show("Opravdu zamíchat?", k.Jmeno, MessageBoxButton.OKCancel);
                        if (res == MessageBoxResult.OK)
                        {
                            Karty[k.Cislo].Clear();
                            Karty[k.Cislo].AddRange(Shuffle(KartyOrg[k.Cislo].ToList()));
                        //reset image
                        Obrazek = null;
                            k.PocetO = 0;
                            k.PocetK = Karty[k.Cislo].Count;
                        }
                    };
                    k.image.MouseUp += delegate (object sender, MouseButtonEventArgs e)
                    {
                        if (Karty[k.Cislo].Count > 0)
                        {
                            try
                            {
                                var kr = r.Next(Karty[k.Cislo].Count);
                                Obrazek = null;
                                timerK.Change(Properties.Settings.Default.Precvak, Timeout.Infinite);
                                ObrazekNext = new BitmapImage(new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location), Karty[k.Cislo][kr]));
                                Karty[k.Cislo].RemoveAt(kr);
                                k.PocetK--;
                                k.PocetO++;
                            }
                            catch { }
                        }
                        else
                            Obrazek = null;
                    };
                    k.PocetK = Karty[k.Cislo].Count;

                    sp.Children.Add(k);
                    sp.Children.Add(new Separator() { Visibility = Visibility.Hidden, Height = 5 });
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Chyba načtení obrázků\n" + e.Message);
            }
        }

        private void tickK(object state)
        {
            Obrazek = ObrazekNext;
        }

        public List<string> Shuffle(List<string> karty)
        {
            List<string> ret = new List<string>();
            var n = karty.Count;
            for (int i = 0; i < n; i++)
            {
                var k = r.Next(karty.Count);
                ret.Add(karty[k]);
                karty.RemoveAt(k);
            }
            return ret;
        }
    }
}
