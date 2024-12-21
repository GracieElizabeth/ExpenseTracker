using System.Diagnostics;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExpensesDbContext _context;

        public HomeController(ILogger<HomeController> logger, ExpensesDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            // Go to DB _context, then Expenses table, track all values in ToList
            var allExpenses = _context.Expenses.ToList();

            var totalExpenses = allExpenses.Sum(x => x.Value);
            ViewBag.Expenses = totalExpenses;

            return View(allExpenses);
        }

        public IActionResult CreateEditExpense(int? id)
        {
            if(id != null)
            {
                // Editing -> load an expense by id
                var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
                return View(expenseInDb);
            }

            // If there's no id then return view
            return View();
        }

        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            _context.Expenses.Remove(expenseInDb);
            _context.SaveChanges();

            // Return user to expenses page after deleting
            return RedirectToAction("Expenses");
        }

        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if(model.Id == 0)
            {
                // Create
                _context.Expenses.Add(model);
            } else
            {
                // Edit
                _context.Expenses.Update(model);
            }
            _context.SaveChanges();

            return RedirectToAction("Expenses");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
