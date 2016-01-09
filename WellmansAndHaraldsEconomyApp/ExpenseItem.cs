using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellmansAndHaraldsEconomyApp
{
    public class ExpenseItem : INotifyPropertyChanged
    {
        private double _totalValue;
        private double _splitAmount;
        private double _ownAmount;
        private double _otherAmount;

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
        public double SplitAmount
        {
            get { return _splitAmount; }
            set
            {
                if (_splitAmount != value)
                {
                    _splitAmount = value;
                    NotifyPropertyChanged("SplitAmount");
                    CalculateReceiptValues();
                }
            }
        }
        public double OwnAmount
        {
            get { return _ownAmount; }
            set
            {
                if (_ownAmount != value)
                {
                    _ownAmount = value;
                    NotifyPropertyChanged("OwnAmount");
                    CalculateReceiptValues();
                }
            }
        }
        public double OtherAmount
        {
            get { return _otherAmount; }
            set
            {
                if (_otherAmount != value)
                {
                    _otherAmount = value;
                    NotifyPropertyChanged("OtherAmount");
                    CalculateReceiptValues();
                }
            }
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
                SplitAmount = TotalValue - OwnAmount - OtherAmount;
            else if (CalcOwn)
                OwnAmount = TotalValue - SplitAmount - OtherAmount;
            else if (CalcOther)
                OtherAmount = TotalValue - OwnAmount - SplitAmount;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Description, TotalValue);
        }
    }
}
