using System.ComponentModel;
using System.Text.RegularExpressions;

namespace WellmansAndHaraldsEconomyApp
{
    public abstract class ExpenseItem : INotifyPropertyChanged
    {
        public string Description { get; set; }
        public double DebtValue { get; set; }
        public bool ParseError { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected static double ParseExpression(string exprString)
        {
            if (string.IsNullOrEmpty(exprString)) return 0;
            exprString = exprString.Replace(" ", string.Empty);
            if (string.IsNullOrEmpty(exprString)) return 0;

            var opRegex = new Regex(@"[\+-]");
            var match1 = opRegex.Match(exprString);
            if (match1.Index == 0)
            {
                return double.Parse(exprString);
            }
            var match2 = opRegex.Match(exprString, match1.Index + 1);

            var result = double.Parse(exprString.Substring(0, match1.Index));
            double nextValue;
            while (match1.Index > 0 && match2.Index > 0)
            {
                nextValue = double.Parse(exprString.Substring(match1.Index + 1, match2.Index - match1.Index - 1));
                if (match1.Value == "-")
                {
                    nextValue = -nextValue;
                }
                result += nextValue;
                match1 = match2;
                match2 = opRegex.Match(exprString, match1.Index + 1);
            }
            nextValue = double.Parse(exprString.Substring(match1.Index + 1));
            if (match1.Value == "-")
            {
                nextValue = -nextValue;
            }
            result += nextValue;
            return result;
        }
    }
}
