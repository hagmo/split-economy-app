namespace WellmansAndHaraldsEconomyApp
{
    public class Receipt : ExpenseItem
    {
        public Receipt() : base()
        {
        }

        public Receipt(string expr)
        {
            expr = expr.Trim();
            var colonIndex = expr.IndexOf(':');
            Description = expr.Substring(0, colonIndex);
            var slashIndex = expr.IndexOf('/');
            var valueString = slashIndex == -1 ? expr.Substring(colonIndex + 1) : expr.Substring(colonIndex + 1, slashIndex - colonIndex - 1);
            SharedValue = ParseExpression(valueString);

            if (slashIndex != -1)
            {
                DebtValue = ParseExpression(expr.Substring(slashIndex + 1));
            }
        }

        public double SharedValue { get; set; }

        public override string ToString()
        {
            if (ParseError)
                return Description;
            if (DebtValue > 0)
                return string.Format("{0}: {1:0.00}/{2:0.00}", Description, SharedValue, DebtValue);
            return string.Format("{0}: {1:0.00}", Description, SharedValue);
        }
    }
}
