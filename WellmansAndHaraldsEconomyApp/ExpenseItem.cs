using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellmansAndHaraldsEconomyApp
{
    public class ExpenseItem
    {
        public string Description { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            string description = Description;
            if (string.IsNullOrEmpty(Description))
                description = "Receipt";

            return string.Format("{0} ({1})", description, Value);
        }
    }
}
