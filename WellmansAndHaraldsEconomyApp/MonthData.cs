using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellmansAndHaraldsEconomyApp
{
    public class MonthData : INotifyPropertyChanged
    {
        #region fields
        private double m_Rent;
        private double m_HGF;
        private double m_Insurance;

        private double m_Broadband;
        private double m_Electricity;
        #endregion

        public MonthData()
        {
            m_Rent = 8397;
            m_HGF = 80;
            m_Insurance = 161;
            m_Broadband = 369;

            WellmanReceipts = new ObservableCollection<ExpenseItem>();
            HaraldDebts = new ObservableCollection<ExpenseItem>();
            HaraldReceipts = new ObservableCollection<ExpenseItem>();
            WellmanDebts = new ObservableCollection<ExpenseItem>();

            CurrentMonth = DateTime.Now.Month - 1;
            CurrentYear = DateTime.Now.Year;
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

        public ObservableCollection<ExpenseItem> WellmanReceipts { get; set; }
        public ObservableCollection<ExpenseItem> HaraldDebts { get; set; }
        public ObservableCollection<ExpenseItem> HaraldReceipts { get; set; }
        public ObservableCollection<ExpenseItem> WellmanDebts { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion properties

        #region addremove
        public void AddHaraldReceipt(ExpenseItem item)
        {
            HaraldReceipts.Add(item);
            NotifyPropertyChanged("ResultValue");
        }

        public void AddHaraldDebt(ExpenseItem item)
        {
            HaraldDebts.Add(item);
            NotifyPropertyChanged("ResultValue");
        }

        public void AddWellmanReceipt(ExpenseItem item)
        {
            WellmanReceipts.Add(item);
            NotifyPropertyChanged("ResultValue");
        }

        public void AddWellmanDebt(ExpenseItem item)
        {
            WellmanDebts.Add(item);
            NotifyPropertyChanged("ResultValue");
        }

        public void RemoveHaraldReceipt(ExpenseItem item)
        {
            HaraldReceipts.Remove(item);
            NotifyPropertyChanged("ResultValue");
        }

        public void RemoveHaraldDebt(ExpenseItem item)
        {
            HaraldDebts.Remove(item);
            NotifyPropertyChanged("ResultValue");
        }

        public void RemoveWellmanReceipt(ExpenseItem item)
        {
            WellmanReceipts.Remove(item);
            NotifyPropertyChanged("ResultValue");
        }

        public void RemoveWellmanDebt(ExpenseItem item)
        {
            WellmanDebts.Remove(item);
            NotifyPropertyChanged("ResultValue");
        }
        #endregion

        public string GetSaveString()
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
            sb.AppendLine("END");

            return sb.ToString();
        }

        public static MonthData Parse(System.IO.StreamReader stream)
        {
            var monthData = new MonthData();

            try
            {
                var line = stream.ReadLine();
                int sepIndex = line.IndexOf('-');
                int subLength2 = line.Length - sepIndex - 2;
                monthData.CurrentMonth = int.Parse(line.Substring(1, sepIndex - 1)) - 1;
                monthData.CurrentYear = int.Parse(line.Substring(sepIndex + 1, subLength2));
                line = stream.ReadLine();

                while (!stream.EndOfStream && !line.StartsWith("["))
                {
                    sepIndex = line.IndexOf('=');
                    subLength2 = line.Length - sepIndex - 1;
                    if (line.StartsWith("Rent"))
                        monthData.Rent = int.Parse(line.Substring(sepIndex + 1, subLength2));
                    else if (line.StartsWith("HGF"))
                        monthData.HGF = int.Parse(line.Substring(sepIndex + 1, subLength2));
                    else if (line.StartsWith("Insurance"))
                        monthData.Insurance = int.Parse(line.Substring(sepIndex + 1, subLength2));
                    else if (line.StartsWith("Broadband"))
                        monthData.Broadband = int.Parse(line.Substring(sepIndex + 1, subLength2));
                    else if (line.StartsWith("Electricity"))
                        monthData.Electricity = int.Parse(line.Substring(sepIndex + 1, subLength2));
                    line = stream.ReadLine();
                }

                var heading = line;
                line = stream.ReadLine();
                while (!stream.EndOfStream)
                {
                    while (line.StartsWith("[") && !stream.EndOfStream)
                    {
                        heading = line;
                        if (!stream.EndOfStream)
                            line = stream.ReadLine();
                    }

                    while (!line.StartsWith("[") && !stream.EndOfStream && line != "END")
                    {
                        sepIndex = line.IndexOf('=');
                        subLength2 = line.Length - sepIndex - 1;
                        var item = new ExpenseItem()
                            {
                                Description = line.Substring(0, sepIndex),
                                Value = double.Parse(line.Substring(sepIndex + 1, subLength2))
                            };
                        if (heading == "[HaraldReceipts]")
                            monthData.HaraldReceipts.Add(item);
                        else if (heading == "[HaraldDebts]")
                            monthData.HaraldDebts.Add(item);
                        else if (heading == "[WellmanReceipts]")
                            monthData.WellmanReceipts.Add(item);
                        else if (heading == "[WellmanDebts]")
                            monthData.WellmanDebts.Add(item);

                        if (!stream.EndOfStream)
                            line = stream.ReadLine();
                    }
                }
            }
            catch
            {
                return null;
            }
            return monthData;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", CurrentMonth + 1, CurrentYear);
        }
    }
}
