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
using System.IO;

namespace doga
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

        //2
        public class Szeleromu
        {
            public string Helyszin { get; set; }
            public int Egyseg { get; set; }
            public int Teljesitmeny { get; set; }
            public int KezdesEv { get; set; }

            //7
            public char GetKategoria()
            {
                if (Teljesitmeny >= 1000)
                {
                    return 'A';
                }
                else if (Teljesitmeny > 500 && Teljesitmeny < 1000)
                {
                    return 'B';
                }
                else
                {
                    return 'C';
                }
            }
        }

        //3
        private List<Szeleromu> eromuvek;

        private void BeolvasasButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                eromuvek = new List<Szeleromu>();
                string[] sorok = File.ReadAllLines("szeleromu.txt");

                foreach (string sor in sorok)
                {
                    string[] adatok = sor.Split(';');

                    if (adatok.Length == 4)
                    {
                        Szeleromu eromu = new Szeleromu
                        {
                            Helyszin = adatok[0],
                            Egyseg = int.Parse(adatok[1]),
                            Teljesitmeny = int.Parse(adatok[2]),
                            KezdesEv = int.Parse(adatok[3])
                        };

                        eromuvek.Add(eromu);
                    }
                }

                MessageBox.Show("Az adatok sikeresen be lettek olvasva!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt az adatok beolvasása közben: {ex.Message}");
            }
        }

        //4
        private void MegjelenitesButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = eromuvek;
        }

        //5
        private void AdatokSzama_Click(object sender, RoutedEventArgs e)
        {
            int adatokSzama = eromuvek.Count;
            MessageBox.Show($"Az állományban összesen {adatokSzama} szélerőmű adat van.");
        }

        //6
        private void SzamolasButton_Click(object sender, RoutedEventArgs e)
        {
            string keresettHelyszin = HelyszinTextBox.Text;
            var eromuvekHelyszinAlapjan = eromuvek.Where(eromu => eromu.Helyszin == keresettHelyszin).ToList();

            EromuvekListBox.ItemsSource = eromuvekHelyszinAlapjan;
            EromuvekSzamaLabel.Content = $"A listában {eromuvekHelyszinAlapjan.Count} szélerőmű van.";
        }

        //8
        private void AtlagosTeljesitmenyButton_Click(object sender, RoutedEventArgs e)
        {
            var eromuvek2010 = eromuvek.Where(eromu => eromu.KezdesEv == 2010).ToList();

            if (eromuvek2010.Count > 0)
            {
                double atlagTeljesitmeny = eromuvek2010.Average(eromu => eromu.Teljesitmeny);
                MessageBox.Show($"A 2010-ben telepített erőművek átlagos teljesítménye: {atlagTeljesitmeny:F2} W.");
            }
            else
            {
                MessageBox.Show("Nincs 2010-ben telepített erőmű.");
            }
        }

        //9
        private void LegnagyobbTeljesitmenyButton_Click(object sender, RoutedEventArgs e)
        {
            var legnagyobbTeljesitmenyu = eromuvek.OrderByDescending(eromu => eromu.Teljesitmeny).FirstOrDefault();

            if (legnagyobbTeljesitmenyu != null)
            {
                MessageBox.Show($"A legnagyobb teljesítményű erőmű:\nHelyszín: {legnagyobbTeljesitmenyu.Helyszin}\nTeljesítmény: {legnagyobbTeljesitmenyu.Teljesitmeny} W\nTelepítés év: {legnagyobbTeljesitmenyu.KezdesEv}");
            }
            else
            {
                MessageBox.Show("Nincs adat az erőművekről.");
            }
        }

        //10
        private void JelentesButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder jelentes = new StringBuilder();

            foreach (var eromu in eromuvek)
            {
                char kategoria = eromu.GetKategoria();
                jelentes.AppendLine($"{eromu.Helyszin},{eromu.Egyseg},{eromu.Teljesitmeny},{kategoria}");
            }

            File.WriteAllText("jelentes.txt", jelentes.ToString());
            MessageBox.Show("A jelentés elkészült és mentve lett a jelentes.txt fájlba.");
        }
    }
}
