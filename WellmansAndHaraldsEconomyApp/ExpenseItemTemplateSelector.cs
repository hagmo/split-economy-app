using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WellmansAndHaraldsEconomyApp
{
    internal class ExpenseItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ExtendedTemplate { get; set; }
        public DataTemplate BasicTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var expenseItem = item as ExpenseItem;
            if (double.Parse(expenseItem.SplitAmountString) != expenseItem.TotalValue)
                return ExtendedTemplate;
            else
                return BasicTemplate;
        }
    }
}
