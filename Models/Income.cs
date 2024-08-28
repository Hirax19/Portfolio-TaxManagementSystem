using System;
using System.ComponentModel.DataAnnotations;

namespace TaxManagementSystem.Models
{
    public class Income
    {
        public int Id { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Client { get; set; }

        public string UserId { get; set; }

        public bool IsExpense { get; set; } = false;
    }
}
