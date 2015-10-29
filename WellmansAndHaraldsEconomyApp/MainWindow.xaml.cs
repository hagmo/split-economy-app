using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            WellmanReceipts = new ObservableCollection<ExpenseItem>();
            NewWellmanReceipt = new ExpenseItem();
            InitializeComponent();
        }

        public double Rent { get; set; }
        public double HGF { get; set; }
        public double Insurance { get; set; }

        public ExpenseItem NewWellmanReceipt { get; set; }
        public ObservableCollection<ExpenseItem> WellmanReceipts { get; set; }
        public ExpenseItem SelectedWellmanReceipt { get; set; }

        private void ViewMonthButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveWellmanReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWellmanReceipt != null)
            {
                WellmanReceipts.Remove(SelectedWellmanReceipt);
            }
        }

        private void AddWellmanReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewWellmanReceipt.Value == 0)
                return;
            WellmanReceipts.Add(NewWellmanReceipt);
            NewWellmanReceipt = new ExpenseItem();
        }
    }
}
