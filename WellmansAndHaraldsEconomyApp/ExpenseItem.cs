using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WellmansAndHaraldsEconomyApp
{
    public class ExpenseItem : INotifyPropertyChanged
    {
        private double _totalValue;
        private string _splitAmountString;
        private string _ownAmountString;
        private string _otherAmountString;

        private bool _calcSplit;
        private bool _calcOwn;
        private bool _calcOther;

        public ExpenseItem()
        {
            CalcSplit = true;
        }

        public string Description { get; set; }
        
        public double TotalValue
        {
            get { return _totalValue; }
            set
            {
                if (_totalValue != value)
                {
                    _totalValue = value;
                    NotifyPropertyChanged("TotalValue");
                    CalculateReceiptValues();
                }
            }
        }

        public string SplitAmountString
        {
            get { return _splitAmountString; }
            set
            {
                if (_splitAmountString != value)
                {
                    _splitAmountString = value;
                    NotifyPropertyChanged("SplitAmountString");
                    CalculateReceiptValues();
                }
            }
        }

        public double SplitAmount
        {
            get { return ParseExpression(SplitAmountString) ?? 0; }
        }

        public string OwnAmountString
        {
            get { return _ownAmountString; }
            set
            {
                if (_ownAmountString != value)
                {
                    _ownAmountString = value;
                    NotifyPropertyChanged("OwnAmountString");
                    CalculateReceiptValues();
                }
            }
        }
        public double OwnAmount
        {
            get { return ParseExpression(OwnAmountString) ?? 0; }
        }

        public string OtherAmountString
        {
            get { return _otherAmountString; }
            set
            {
                if (_otherAmountString != value)
                {
                    _otherAmountString = value;
                    NotifyPropertyChanged("OtherAmountString");
                    CalculateReceiptValues();
                }
            }
        }
        public double OtherAmount
        {
            get { return ParseExpression(OtherAmountString) ?? 0; }
        }

        public bool CalcSplit
        {
            get { return _calcSplit; }
            set
            {
                if (_calcSplit != value)
                {
                    _calcSplit = value;
                    NotifyPropertyChanged("CalcSplit");
                    CalculateReceiptValues();
                }
            }
        }

        public bool CalcOwn
        {
            get { return _calcOwn; }
            set
            {
                if (_calcOwn != value)
                {
                    _calcOwn = value;
                    NotifyPropertyChanged("CalcOwn");
                    CalculateReceiptValues();
                }
            }
        }

        public bool CalcOther
        {
            get { return _calcOther; }
            set
            {
                if (_calcOther != value)
                {
                    _calcOther = value;
                    NotifyPropertyChanged("CalcOther");
                    CalculateReceiptValues();
                }
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

        private void CalculateReceiptValues()
        {
            if (CalcSplit)
                SplitAmountString = (TotalValue - OwnAmount - OtherAmount).ToString("F");
            else if (CalcOwn)
                OwnAmountString = (TotalValue - SplitAmount - OtherAmount).ToString("F");
            else if (CalcOther)
                OtherAmountString = (TotalValue - OwnAmount - SplitAmount).ToString("F");
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Description, TotalValue);
        }

        private static double? ParseExpression(string exprString)
        {
            if (string.IsNullOrEmpty(exprString)) return 0;

            exprString = exprString.Replace(" ", string.Empty);
            var opRegex = new Regex(@"[\+-]");
            var match1 = opRegex.Match(exprString);
            if (match1.Index == 0)
            {
                return double.Parse(exprString);
            }
            var match2 = opRegex.Match(exprString, match1.Index + 1);

            var result = double.Parse(exprString.Substring(0, match1.Index));
            try
            {
                while (match1.Index > 0 && match2.Index > 0)
                {
                    result += int.Parse(exprString.Substring(match1.Index, match2.Index - match1.Index));
                    match1 = match2;
                    match2 = opRegex.Match(exprString, match1.Index + 1);
                }
                result += double.Parse(exprString.Substring(match1.Index));
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }
    }
}
