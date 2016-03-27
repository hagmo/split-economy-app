using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            PreviousMonthData = new ObservableCollection<MonthData>();

            InitializeComponent();
        }

        #region properties

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

        public string HaraldReceiptsString
        {
            get { return ExpenseItemListString(CurrentMonthData.HaraldReceipts); }
            set
            {
                UpdateExpenseItemList(CurrentMonthData.HaraldReceipts, value);
                NotifyPropertyChanged("HaraldReceiptsString");
            }
        }

        public string WellmanDebtsString
        {
            get { return ExpenseItemListString(CurrentMonthData.WellmanDebts); }
            set
            {
                UpdateExpenseItemList(CurrentMonthData.WellmanDebts, value);
                NotifyPropertyChanged("WellmanDebtsString");
            }
        }

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
            var textBox = sender as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentMonthData = new MonthData();
        }

        private string ExpenseItemListString(IEnumerable<ExpenseItem> items)
        {
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.AppendLine(item.ToString());
            }
            if (sb.Length >= 2)
            {
                sb.Replace(Environment.NewLine, string.Empty, sb.Length - 2, 2);
            }
            return sb.ToString();
        }

        private void UpdateExpenseItemList(ICollection<Receipt> items, string value)
        {
            items.Clear();
            var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                items.Add(new Receipt(line));
            }
        }

        private void UpdateExpenseItemList(ICollection<DebtItem> items, string value)
        {
            items.Clear();
            var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                items.Add(new DebtItem(line));
            }
        }

        private void HaraldReceipts_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Key != Key.Enter && e.Key != Key.Return || textBox == null)
                return;

            HaraldReceiptsString = textBox.Text;

            textBox.CaretIndex = textBox.Text.Length;
        }

        private void WellmanDebts_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Key != Key.Enter && e.Key != Key.Return || textBox == null)
                return;

            WellmanDebtsString = textBox.Text;

            textBox.CaretIndex = textBox.Text.Length;
        }
    }
}
