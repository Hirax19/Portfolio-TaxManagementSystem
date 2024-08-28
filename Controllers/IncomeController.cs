using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxManagementSystem.Data;
using TaxManagementSystem.Models;

namespace TaxManagementSystem.Controllers
{
    public class IncomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Income
        public async Task<IActionResult> Index(string sortOrder)
        {
            // Sorting logic
            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.AmountSortParam = sortOrder == "Amount" ? "amount_desc" : "Amount";
            ViewBag.SourceSortParam = sortOrder == "Source" ? "source_desc" : "Source";
            ViewBag.ClientSortParam = sortOrder == "Client" ? "client_desc" : "Client";

            var incomes = from i in _context.Incomes
                          select i;

            switch (sortOrder)
            {
                case "date_desc":
                    incomes = incomes.OrderByDescending(i => i.Date);
                    break;
                case "Amount":
                    incomes = incomes.OrderBy(i => i.Amount);
                    break;
                case "amount_desc":
                    incomes = incomes.OrderByDescending(i => i.Amount);
                    break;
                case "Source":
                    incomes = incomes.OrderBy(i => i.Source);
                    break;
                case "source_desc":
                    incomes = incomes.OrderByDescending(i => i.Source);
                    break;
                case "Client":
                    incomes = incomes.OrderBy(i => i.Client);
                    break;
                case "client_desc":
                    incomes = incomes.OrderByDescending(i => i.Client);
                    break;
                default:
                    incomes = incomes.OrderBy(i => i.Date);
                    break;
            }

            return View(await incomes.AsNoTracking().ToListAsync());
        }

        // GET: Income/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // GET: Income/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Income/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Source,Amount,Date,Client,IsExpense,UserId")] Income income)
        {
            if (ModelState.IsValid)
            {
                _context.Add(income);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(income);
        }

        // GET: Income/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }
            return View(income);
        }

        // POST: Income/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Source,Amount,Date,Client,IsExpense,UserId")] Income income)
        {
            if (id != income.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(income);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomeExists(income.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(income);
        }

        // GET: Income/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // POST: Income/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income != null)
            {
                _context.Incomes.Remove(income);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool IncomeExists(int id)
        {
            return _context.Incomes.Any(e => e.Id == id);
        }
    }
}
