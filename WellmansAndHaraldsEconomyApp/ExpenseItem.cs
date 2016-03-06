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
        public ExpenseItem()
        {
        }

        public ExpenseItem(string expr, bool debt = false)
        {
            try
            {
                expr = expr.Trim();
                var colonIndex = expr.IndexOf(':');
                Description = expr.Substring(0, colonIndex);
                var slashIndex = expr.IndexOf('/');
                var valueString = slashIndex == -1 ? expr.Substring(colonIndex + 1) : expr.Substring(colonIndex + 1, slashIndex - colonIndex - 1);

                if (!debt)
                {
                    SharedValue = ParseExpression(valueString);
                }
                else
                {
                    DebtValue = ParseExpression(valueString);
                }

                if (!debt && slashIndex != -1)
                {
                    DebtValue = ParseExpression(expr.Substring(slashIndex + 1));
                }
                else if (debt)
                {
                    ParseError = true;
                }
            }
            catch
            {
                ParseError = true;
            }
            
            if (ParseError)
            {
                Description = expr;
            }
        }

        public string Description { get; set; }
        public double SharedValue { get; set; }
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

        public override string ToString()
        {
            if (ParseError)
                return Description;
            if (DebtValue > 0)
                return string.Format("{0}: {1:0.00}/{2:0.00}", Description, SharedValue, DebtValue);
            return string.Format("{0}: {1:0.00}", Description, SharedValue);
        }

        private static double ParseExpression(string exprString)
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
