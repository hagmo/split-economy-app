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

namespace WellmansAndHaraldsEconomyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region fields
        private double m_Rent;
        private double m_HGF;
        private double m_Insurance;

        private double m_Broadband;
        private double m_Electricity;
        #endregion

        public MainWindow()
        {
            m_Rent = 8397;
            m_HGF = 80;
            m_Insurance = 161;
            m_Broadband = 369;

            WellmanReceipts = new ObservableCollection<ExpenseItem>();
            NewWellmanReceipt = new ExpenseItem();
            HaraldDebts = new ObservableCollection<ExpenseItem>();
            NewHaraldDebt = new ExpenseItem();

            HaraldReceipts = new ObservableCollection<ExpenseItem>();
            NewHaraldReceipt = new ExpenseItem();
            WellmanDebts = new ObservableCollection<ExpenseItem>();
            NewWellmanDebt = new ExpenseItem();

            CurrentMonth = DateTime.Now.Month - 1;
            CurrentYear = DateTime.Now.Year;

            InitializeComponent();
        }

        #region properties
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }

        public double Rent
        {
            get { return m_Rent; }
            set
            {
                if (m_Rent != value)
                {
                    m_Rent = value;
                    NotifyPropertyChanged("ResultValue");
                }
            }
        }
        public double HGF
        {
            get { return m_HGF; }
            set
            {
                if (m_HGF != value)
                {
                    m_HGF = value;
                    NotifyPropertyChanged("ResultValue");
                }
            }
        }
        public double Insurance
        {
            get { return m_Insurance; }
            set
            {
                if (m_Insurance != value)
                {
                    m_Insurance = value;
                    NotifyPropertyChanged("ResultValue");
                }
            }
        }

        public double Broadband
        {
            get { return m_Broadband; }
            set
            {
                if (m_Broadband != value)
                {
                    m_Broadband = value;
                    NotifyPropertyChanged("ResultValue");
                }
            }
        }
        public double Electricity
        {
            get { return m_Electricity; }
            set
            {
                if (m_Electricity != value)
                {
                    m_Electricity = value;
                    NotifyPropertyChanged("ResultValue");
                }
            }
        }

        public ExpenseItem NewWellmanReceipt { get; set; }
        public ObservableCollection<ExpenseItem> WellmanReceipts { get; set; }
        public ExpenseItem SelectedWellmanReceipt { get; set; }

        public ExpenseItem NewHaraldDebt { get; set; }
        public ObservableCollection<ExpenseItem> HaraldDebts { get; set; }
        public ExpenseItem SelectedHaraldDebt { get; set; }

        public ExpenseItem NewHaraldReceipt { get; set; }
        public ObservableCollection<ExpenseItem> HaraldReceipts { get; set; }
        public ExpenseItem SelectedHaraldReceipt { get; set; }

        public ExpenseItem NewWellmanDebt { get; set; }
        public ObservableCollection<ExpenseItem> WellmanDebts { get; set; }
        public ExpenseItem SelectedWellmanDebt { get; set; }

        public double ResultValue
        {
            get
            {
                double result = 0;

                result += Rent / 2d;
                result += HGF / 2d;
                result += Insurance / 2d;

                result -= Electricity / 2d;
                result -= Broadband / 2d;

                foreach (var item in WellmanReceipts)
                {
                    result += item.Value / 2d;
                }
                foreach (var item in HaraldDebts)
                {
                    result += item.Value;
                }
                foreach (var item in HaraldReceipts)
                {
                    result -= item.Value / 2d;
                }
                foreach (var item in WellmanDebts)
                {
                    result -= item.Value;
                }
                return result;
            }
        }
        #endregion properties

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ViewMonthButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddWellmanReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewWellmanReceipt.Value == 0)
                return;
            WellmanReceipts.Add(new ExpenseItem()
            {
                Description = NewWellmanReceipt.Description,
                Value = NewWellmanReceipt.Value
            });
            NotifyPropertyChanged("ResultValue");
        }

        private void RemoveWellmanReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWellmanReceipt != null)
            {
                WellmanReceipts.Remove(SelectedWellmanReceipt);
                NotifyPropertyChanged("ResultValue");
            }
        }

        private void AddHaraldDebtButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewHaraldDebt.Value == 0)
                return;
            HaraldDebts.Add(new ExpenseItem()
            {
                Description = NewHaraldDebt.Description,
                Value = NewHaraldDebt.Value
            });
            NotifyPropertyChanged("ResultValue");
        }

        private void RemoveHaraldDebtButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedHaraldDebt != null)
            {
                HaraldDebts.Remove(SelectedHaraldDebt);
                NotifyPropertyChanged("ResultValue");
            }
        }

        private void AddHaraldReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewHaraldReceipt.Value == 0)
                return;
            HaraldReceipts.Add(new ExpenseItem()
            {
                Description = NewHaraldReceipt.Description,
                Value = NewHaraldReceipt.Value
            });
            NotifyPropertyChanged("ResultValue");
        }

        private void RemoveHaraldReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedHaraldReceipt != null)
            {
                HaraldReceipts.Remove(SelectedHaraldReceipt);
                NotifyPropertyChanged("ResultValue");
            }
        }

        private void AddWellmanDebtButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewWellmanDebt.Value == 0)
                return;
            WellmanDebts.Add(new ExpenseItem()
            {
                Description = NewWellmanDebt.Description,
                Value = NewWellmanDebt.Value
            });
            NotifyPropertyChanged("ResultValue");
        }

        private void RemoveWellmanDebtButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWellmanDebt != null)
            {
                WellmanDebts.Remove(SelectedWellmanDebt);
                NotifyPropertyChanged("ResultValue");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("[{0}-{1}]", CurrentMonth + 1, CurrentYear);
            sb.AppendLine();
            sb.AppendFormat("Rent={0}", Rent);
            sb.AppendLine();
            sb.AppendFormat("HGF={0}", HGF);
            sb.AppendLine();
            sb.AppendFormat("Insurance={0}", Insurance);
            sb.AppendLine();
            sb.AppendFormat("Broadband={0}", Broadband);
            sb.AppendLine();
            sb.AppendFormat("Electricity={0}", Electricity);
            sb.AppendLine();
            sb.AppendLine("[HaraldReceipts]");
            foreach (var item in HaraldReceipts)
            {
                sb.AppendFormat("{0}={1}", item.Description, item.Value);
                sb.AppendLine();
            }
            sb.AppendLine("[HaraldDebts]");
            foreach (var item in HaraldDebts)
            {
                sb.AppendFormat("{0}={1}", item.Description, item.Value);
                sb.AppendLine();
            }
            sb.AppendLine("[WellmanReceipts]");
            foreach (var item in WellmanReceipts)
            {
                sb.AppendFormat("{0}={1}", item.Description, item.Value);
                sb.AppendLine();
            }
            sb.AppendLine("[WellmanDebts]");
            foreach (var item in WellmanDebts)
            {
                sb.AppendFormat("{0}={1}", item.Description, item.Value);
                sb.AppendLine();
            }


            string dirPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WellmansAndHaraldsEconomyApp");
            System.IO.Directory.CreateDirectory(dirPath);
            using (var writer = new System.IO.StreamWriter(System.IO.Path.Combine(dirPath, string.Format("file{0}-{1}", CurrentYear, CurrentMonth + 1))))
            {
                writer.Write(sb.ToString());
            }
        }
    }
}
