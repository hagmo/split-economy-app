namespace WellmansAndHaraldsEconomyApp
{
    public class DebtItem : ExpenseItem
    {
        public DebtItem() : base()
        {
        }

        public DebtItem(string expr)
        {
            try
            {
                expr = expr.Trim();
                var colonIndex = expr.IndexOf(':');
                Description = expr.Substring(0, colonIndex);
                var valueString = expr.Substring(colonIndex + 1);
                DebtValue = ParseExpression(valueString);
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

        public override string ToString()
        {
            if (ParseError)
                return Description;
            return string.Format("{0}: {1:0.00}", Description, DebtValue);
        }
    }
}
