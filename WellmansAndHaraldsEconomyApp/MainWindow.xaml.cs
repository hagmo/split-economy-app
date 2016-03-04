using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WellmansAndHaraldsEconomyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private MonthData _currentMonthData;

        private static readonly string SaveDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WellmansAndHaraldsEconomyApp");

        public MainWindow()
        {
            _currentMonthData = new MonthData();

            NewWellmanReceipt = new ExpenseItem();
            NewHaraldDebt = new ExpenseItem();
            NewHaraldReceipt = new ExpenseItem();
            NewWellmanDebt = new ExpenseItem();

            PreviousMonthData = new ObservableCollection<MonthData>();

            InitializeComponent();
        }

        #region properties
        public ExpenseItem NewWellmanReceipt { get; set; }
        public ExpenseItem SelectedWellmanReceipt { get; set; }

        public ExpenseItem NewHaraldDebt { get; set; }
        public ExpenseItem SelectedHaraldDebt { get; set; }

        public ExpenseItem NewHaraldReceipt { get; set; }
        public ExpenseItem SelectedHaraldReceipt { get; set; }

        public ExpenseItem NewWellmanDebt { get; set; }
        public ExpenseItem SelectedWellmanDebt { get; set; }

        public MonthData SelectedMonthData { get; set; }

        public MonthData CurrentMonthData
        {
            get { return _currentMonthData; }
            set
            {
                if (_currentMonthData != value)
                {
                    _currentMonthData = value;
                    NotifyPropertyChanged("CurrentMonthData");
                }
            }
        }

        public ObservableCollection<MonthData> PreviousMonthData { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void ViewMonthButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentMonthData = SelectedMonthData;
        }

        private void AddWellmanReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewWellmanReceipt.TotalValue == 0 || string.IsNullOrEmpty(NewWellmanReceipt.Description))
                return;
            CurrentMonthData.AddWellmanReceipt(new ExpenseItem()
            {
                Description = NewWellmanReceipt.Description,
                TotalValue = NewWellmanReceipt.TotalValue,
                SplitAmountString = NewWellmanReceipt.SplitAmountString,
                OwnAmountString = NewWellmanReceipt.OwnAmountString,
                OtherAmountString = NewWellmanReceipt.OtherAmountString
            });
        }

        private void RemoveWellmanReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWellmanReceipt != null)
                CurrentMonthData.RemoveWellmanReceipt(SelectedWellmanReceipt);
        }

        private void AddHaraldDebtButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewHaraldDebt.TotalValue == 0 || string.IsNullOrEmpty(NewHaraldDebt.Description))
                return;
            CurrentMonthData.AddHaraldDebt(new ExpenseItem()
            {
                Description = NewHaraldDebt.Description,
                TotalValue = NewHaraldDebt.TotalValue
            });
        }

        private void RemoveHaraldDebtButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedHaraldDebt != null)
                CurrentMonthData.RemoveHaraldDebt(SelectedHaraldDebt);
        }

        private void AddHaraldReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewHaraldReceipt.TotalValue == 0 || string.IsNullOrEmpty(NewHaraldReceipt.Description))
                return;
            CurrentMonthData.AddHaraldReceipt(new ExpenseItem()
            {
                Description = NewHaraldReceipt.Description,
                TotalValue = NewHaraldReceipt.TotalValue,
                SplitAmountString = NewHaraldReceipt.SplitAmountString,
                OwnAmountString = NewHaraldReceipt.OwnAmountString,
                OtherAmountString = NewHaraldReceipt.OtherAmountString
            });
        }

        private void RemoveHaraldReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedHaraldReceipt != null)
                CurrentMonthData.RemoveHaraldReceipt(SelectedHaraldReceipt);
        }

        private void AddWellmanDebtButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewWellmanDebt.TotalValue == 0 || string.IsNullOrEmpty(NewWellmanDebt.Description))
                return;
            CurrentMonthData.AddWellmanDebt(new ExpenseItem()
            {
                Description = NewWellmanDebt.Description,
                TotalValue = NewWellmanDebt.TotalValue
            });
        }

        private void RemoveWellmanDebtButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWellmanDebt != null)
                CurrentMonthData.RemoveWellmanDebt(SelectedWellmanDebt);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var goAhead = true;
            var oldData = PreviousMonthData.SingleOrDefault(x => x.CurrentMonth == CurrentMonthData.CurrentMonth && x.CurrentYear == CurrentMonthData.CurrentYear);
            if (oldData != null)
            {
                var result = MessageBox.Show("Do you want to overwrite the old data for this month?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    PreviousMonthData.Remove(oldData);
                else
                    goAhead = false;
            }

            if (goAhead)
            {
                PreviousMonthData.Add(CurrentMonthData);

                Directory.CreateDirectory(SaveDir);
                using (var writer = new StreamWriter(Path.Combine(SaveDir, string.Format("file{0}-{1}", CurrentMonthData.CurrentYear, CurrentMonthData.CurrentMonth + 1))))
                {
                    writer.Write(CurrentMonthData.GetSaveString());
                }

                CurrentMonthData = new MonthData();
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            var saveDir = new DirectoryInfo(SaveDir);
            if (saveDir.Exists)
            {
                var invalidFiles = new List<string>();
                foreach (var file in saveDir.EnumerateFiles())
                {
                    using (var reader = file.OpenText())
                    {
                        var monthData = MonthData.Parse(reader);
                        if (monthData != null)
                        {
                            PreviousMonthData.Add(monthData);
                        }
                        else
                        {
                            invalidFiles.Add(file.Name);
                        }
                    }
                }
                if (invalidFiles.Count > 0)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("The following files contain invalid data:");
                    foreach (var fileName in invalidFiles)
                        sb.AppendLine(fileName);
                    sb.AppendLine();
                    sb.AppendLine("Would you like to delete them?");

                    if (MessageBox.Show(sb.ToString(), "Invalid files found", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        foreach (var fileName in invalidFiles)
                        {
                            File.Delete(Path.Combine(SaveDir, fileName));
                        }
                    }
                }
            }
        }

        private void Textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentMonthData = new MonthData();
        }
    }
}
