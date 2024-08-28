using System.ComponentModel.DataAnnotations;

namespace TaxManagementSystem.Models
{
    public class TaxCalculationModel
    {
        [Display(Name = "Brutowinst (Gross Profit)")]
        public decimal GrossProfit { get; set; }

        [Display(Name = "Zelfstandigenaftrek (Entrepreneur Deduction)")]
        public bool ApplyZelfstandigenaftrek { get; set; } = true; // Default to true

        [Display(Name = "Startersaftrek (Starter Deduction)")]
        public bool ApplyStartersaftrek { get; set; }

        public decimal ZelfstandigenaftrekAmount { get; set; } = 3750; // For 2024

        public decimal StartersaftrekAmount { get; set; } = 2123; // For 2024

        [Display(Name = "Mkb-winstvrijstelling (SME profit exemption)")]
        public decimal MkbWinstvrijstellingPercentage { get; set; } = 13.31m;

        public decimal TaxableIncome { get; set; }

        public decimal IncomeTax { get; set; }

        public decimal GeneralTaxCredit { get; set; }

        public decimal LaborTaxCredit { get; set; }

        public decimal HealthcareContribution { get; set; }

        public decimal FinalTaxAmount { get; set; }
    }
}
