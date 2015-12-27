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
        private double m_TotalValue;
        private double m_SplitAmount;
        private double m_OwnAmount;
        private double m_OtherAmount;

        private bool m_CalcSplit;
        private bool m_CalcOwn;
        private bool m_CalcOther;

        public ExpenseItem()
        {
            CalcSplit = true;
        }

        public string Description { get; set; }
        public double TotalValue
        {
            get { return m_TotalValue; }
            set
            {
                if (m_TotalValue != value)
                {
                    m_TotalValue = value;
                    NotifyPropertyChanged("TotalValue");
                    CalculateReceiptValues();
                }
            }
        }
        public double SplitAmount
        {
            get { return m_SplitAmount; }
            set
            {
                if (m_SplitAmount != value)
                {
                    m_SplitAmount = value;
                    NotifyPropertyChanged("SplitAmount");
                    CalculateReceiptValues();
                }
            }
        }
        public double OwnAmount
        {
            get { return m_OwnAmount; }
            set
            {
                if (m_OwnAmount != value)
                {
                    m_OwnAmount = value;
                    NotifyPropertyChanged("OwnAmount");
                    CalculateReceiptValues();
                }
            }
        }
        public double OtherAmount
        {
            get { return m_OtherAmount; }
            set
            {
                if (m_OtherAmount != value)
                {
                    m_OtherAmount = value;
                    NotifyPropertyChanged("OtherAmount");
                    CalculateReceiptValues();
                }
            }
        }

        public bool CalcSplit
        {
            get { return m_CalcSplit; }
            set
            {
                if (m_CalcSplit != value)
                {
                    m_CalcSplit = value;
                    NotifyPropertyChanged("CalcSplit");
                    CalculateReceiptValues();
                }
            }
        }

        public bool CalcOwn
        {
            get { return m_CalcOwn; }
            set
            {
                if (m_CalcOwn != value)
                {
                    m_CalcOwn = value;
                    NotifyPropertyChanged("CalcOwn");
                    CalculateReceiptValues();
                }
            }
        }

        public bool CalcOther
        {
            get { return m_CalcOther; }
            set
            {
                if (m_CalcOther != value)
                {
                    m_CalcOther = value;
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
