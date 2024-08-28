using System;
using System.Collections.Generic;

namespace TaxManagementSystem.Models
{
    public class DashboardViewModel
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal EstimatedTax { get; set; }

        public List<Income> Incomes { get; set; }
        public List<Expense> Expenses { get; set; }

        public List<Income> RecentIncomes { get; set; }
        public List<Expense> RecentExpenses { get; set; }

        public TaxCalculationModel TaxCalculation { get; set; }
    }
}
