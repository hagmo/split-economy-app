﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WellmansAndHaraldsEconomyApp
{
    public class MonthData : INotifyPropertyChanged
    {
        #region fields
        private double _rent;
        private double _hgf;
        private double _insurance;

        private double _broadband;
        private double _electricity;
        #endregion

        public MonthData()
        {
            _rent = 8397;
            _hgf = 80;
            _insurance = 161;
            _broadband = 369;

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
            get { return _rent; }
            set
            {
                if (_rent != value)
                {
                    _rent = value;
                    NotifyPropertyChanged("ResultValue");
                }
            }
        }
        public double HGF
        {
            get { return _hgf; }
            set
            {
                if (_hgf != value)
                {
                    _hgf = value;
                    NotifyPropertyChanged("ResultValue");
                }
            }
        }
        public double Insurance
        {
            get { return _insurance; }
            set
            {
                if (_insurance != value)
                {
                    _insurance = value;
                    NotifyPropertyChanged("ResultValue");
                }
            }
        }

        public double Broadband
        {
            get { return _broadband; }
            set
            {
                if (_broadband != value)
                {
                    _broadband = value;
                    NotifyPropertyChanged("ResultValue");
                }
            }
        }
        public double Electricity
        {
            get { return _electricity; }
            set
            {
                if (_electricity != value)
                {
                    _electricity = value;
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
                    result += item.SplitAmount / 2d;
                    result += item.OtherAmount;
                }
                result += HaraldDebts.Sum(item => item.TotalValue);
                foreach (var item in HaraldReceipts)
                {
                    result -= item.SplitAmount / 2d;
                    result -= item.OtherAmount;
                }
                foreach (var item in WellmanDebts)
                {
                    result -= item.TotalValue;
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

        #region saveload
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
                sb.AppendFormat("{0}={1}/{2}/{3}", item.Description, item.SplitAmountString, item.OtherAmountString, item.OwnAmountString);
                sb.AppendLine();
            }
            sb.AppendLine("[HaraldDebts]");
            foreach (var item in HaraldDebts)
            {
                sb.AppendFormat("{0}={1}/{2}/{3}", item.Description, item.SplitAmountString, item.OtherAmountString, item.OwnAmountString);
                sb.AppendLine();
            }
            sb.AppendLine("[WellmanReceipts]");
            foreach (var item in WellmanReceipts)
            {
                sb.AppendFormat("{0}={1}/{2}/{3}", item.Description, item.SplitAmountString, item.OtherAmountString, item.OwnAmountString);
                sb.AppendLine();
            }
            sb.AppendLine("[WellmanDebts]");
            foreach (var item in WellmanDebts)
            {
                sb.AppendFormat("{0}={1}/{2}/{3}", item.Description, item.SplitAmountString, item.OtherAmountString, item.OwnAmountString);
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
                var sepIndex = line.IndexOf('-');
                var subLength2 = line.Length - sepIndex - 2;
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
                    if (line.StartsWith("[") && !stream.EndOfStream)
                    {
                        heading = line;
                        line = stream.ReadLine();
                    }

                    var indices = new int[3];

                    while (!line.StartsWith("[") && !stream.EndOfStream && line != "END")
                    {
                        indices[0] = line.IndexOf('=');
                        indices[1] = line.IndexOf('/');
                        ExpenseItem item = null;
                        if (indices[1] == -1)
                        {
                            item = new ExpenseItem()
                            {
                                Description = line.Substring(0, indices[0]),
                                TotalValue = double.Parse(line.Substring(indices[0] + 1, line.Length - indices[0] - 1))
                            };
                        }
                        else
                        {
                            indices[2] = line.IndexOf('/', indices[1] + 1);
                            var splitAmount = line.Substring(indices[0] + 1, indices[1] - indices[0] - 1);
                            var ownAmount = line.Substring(indices[1] + 1, indices[2] - indices[1] - 1);
                            var otherAmount = line.Substring(indices[2] + 1, line.Length - indices[2] - 1);
                            item = new ExpenseItem()
                            {
                                Description = line.Substring(0, indices[0]),
                                CalcSplit = false,
                                SplitAmountString = splitAmount,
                                OwnAmountString = ownAmount,
                                OtherAmountString = otherAmount
                            };
                            item.TotalValue = item.SplitAmount + item.OwnAmount + item.OtherAmount;
                        }
                        
                        switch (heading)
                        {
                            case "[HaraldReceipts]":
                                monthData.HaraldReceipts.Add(item);
                                break;
                            case "[HaraldDebts]":
                                monthData.HaraldDebts.Add(item);
                                break;
                            case "[WellmanReceipts]":
                                monthData.WellmanReceipts.Add(item);
                                break;
                            case "[WellmanDebts]":
                                monthData.WellmanDebts.Add(item);
                                break;
                        }

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
        #endregion

        public override string ToString()
        {
            return string.Format("{0}-{1}", CurrentMonth + 1, CurrentYear);
        }
    }
}
