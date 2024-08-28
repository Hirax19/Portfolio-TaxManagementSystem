using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TaxManagementSystem.Data;
using TaxManagementSystem.Models;

namespace TaxManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            // Initialize the tax model with default values
            var taxModel = new TaxCalculationModel
            {
                ApplyZelfstandigenaftrek = true, // Default to true for initial load
                ApplyStartersaftrek = false // Default to false for initial load
            };

            // Prepare and return the model
            var model = await PrepareDashboardViewModel(taxModel);
            return View(model);
        }

        // POST: Dashboard
        [HttpPost]
        public async Task<IActionResult> Index(bool applyZelfstandigenaftrek, bool applyStartersaftrek)
        {
            // Initialize the tax model with the values from the form
            var taxModel = new TaxCalculationModel
            {
                ApplyZelfstandigenaftrek = applyZelfstandigenaftrek,
                ApplyStartersaftrek = applyStartersaftrek
            };

            // Prepare and return the model with updated tax calculations
            var model = await PrepareDashboardViewModel(taxModel);
            return View(model);
        }

        private async Task<DashboardViewModel> PrepareDashboardViewModel(TaxCalculationModel taxModel)
        {
            // Fetch all incomes and expenses from the database
            var allIncomes = await _context.Incomes.Where(i => !i.IsExpense).OrderBy(i => i.Date).ToListAsync();
            var allExpenses = await _context.Expenses.OrderBy(e => e.Date).ToListAsync();

            // Calculate gross profit (total income - total expenses)
            decimal grossProfit = allIncomes.Sum(i => i.Amount) - allExpenses.Sum(e => e.Amount);
            taxModel.GrossProfit = grossProfit;

            // Perform tax calculation
            PerformTaxCalculation(taxModel);

            // Create and return the view model
            return new DashboardViewModel
            {
                TotalIncome = allIncomes.Sum(i => i.Amount),
                TotalExpenses = allExpenses.Sum(e => e.Amount),
                EstimatedTax = taxModel.FinalTaxAmount,
                TaxCalculation = taxModel,
                Incomes = allIncomes, // All incomes for chart
                Expenses = allExpenses, // All expenses for chart
                RecentIncomes = allIncomes, // Pass all incomes
                RecentExpenses = allExpenses // Pass all expenses
            };
        }

        private void PerformTaxCalculation(TaxCalculationModel model)
        {
            // Apply deductions
            decimal deductions = 0;

            if (model.ApplyZelfstandigenaftrek)
            {
                deductions += model.ZelfstandigenaftrekAmount;
            }

            if (model.ApplyStartersaftrek)
            {
                deductions += model.StartersaftrekAmount;
            }

            // Calculate taxable income after deductions
            model.TaxableIncome = model.GrossProfit - deductions;

            // Apply SME profit exemption (Mkb-winstvrijstelling)
            model.TaxableIncome -= model.TaxableIncome * (model.MkbWinstvrijstellingPercentage / 100);

            // Determine the tax rate based on taxable income
            decimal taxRate = model.TaxableIncome <= 75518 ? 36.97m : 49.50m;

            // Calculate income tax
            model.IncomeTax = model.TaxableIncome * (taxRate / 100);

            // General Tax Credit and Labor Tax Credit (simplified example)
            model.GeneralTaxCredit = CalculateGeneralTaxCredit(model.TaxableIncome);
            model.LaborTaxCredit = CalculateLaborTaxCredit(model.TaxableIncome);

            // Healthcare contribution
            model.HealthcareContribution = model.TaxableIncome <= 71628 ? model.TaxableIncome * 0.0532m : 71628 * 0.0532m;

            // Final tax amount
            model.FinalTaxAmount = model.IncomeTax - model.GeneralTaxCredit - model.LaborTaxCredit + model.HealthcareContribution;
        }

        private decimal CalculateGeneralTaxCredit(decimal taxableIncome)
        {
            // Example formula based on 2024 figures
            if (taxableIncome <= 24812)
            {
                return 3362;
            }
            else if (taxableIncome <= 75518)
            {
                return 3362 - (0.0663m * (taxableIncome - 24812));
            }
            else
            {
                return 0;
            }
        }

        private decimal CalculateLaborTaxCredit(decimal taxableIncome)
        {
            // Example formula based on 2024 figures
            if (taxableIncome <= 39857)
            {
                return 5532;
            }
            else if (taxableIncome <= 124935)
            {
                return 5532 - (0.0651m * (taxableIncome - 39857));
            }
            else
            {
                return 0;
            }
        }
    }
}
