using CRUDEmployee.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class EmployeesController : Controller
{
    private readonly ApplicationDbContext _context;
    private List<SelectListItem> GetPositions()
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "Manager", Text = "Manager" },
            new SelectListItem { Value = "Developer", Text = "Developer" },
            new SelectListItem { Value = "Designer", Text = "Designer" },
            new SelectListItem { Value = "Tester", Text = "Tester" }
        };
    }
    public EmployeesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Employees.ToListAsync());
    }

    [HttpGet]
    public IActionResult CreateOrEdit(int? id)
    {
        ViewBag.Positions = GetPositions();

        if (id == null)
        {
            return PartialView("_CreateOrEdit");
        }
        else
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return PartialView("_CreateOrEdit", employee);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Save(Employee employee)
    {
        if (ModelState.IsValid)
        {
            if (employee.Id != null)
            {
                _context.Add(employee);
            }
            else
            {
                _context.Update(employee);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        ViewBag.Positions = GetPositions();
        return PartialView("_CreateOrEdit", employee);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

   
}
