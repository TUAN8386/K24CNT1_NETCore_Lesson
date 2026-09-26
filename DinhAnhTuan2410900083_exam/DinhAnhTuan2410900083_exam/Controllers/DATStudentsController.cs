
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DinhAnhTuan2410900083_exam.Models;
using DinhAnhTuan2410900083_exam.Data;

public class DATStudentsController : Controller
{
    private readonly AppDbContext _context;

    public DATStudentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: DATSTUDENTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.DATStudents.ToListAsync());
    }

    // GET: DATSTUDENTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var datstudent = await _context.DATStudents
            .FirstOrDefaultAsync(m => m.Id == id);
        if (datstudent == null)
        {
            return NotFound();
        }

        return View(datstudent);
    }

    // GET: DATSTUDENTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DATSTUDENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,DATName,DATGender,DATBirthDay,DATEmail,DATPhone,DATActive")] DATStudent datstudent)
    {
        if (ModelState.IsValid)
        {
            _context.Add(datstudent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(datstudent);
    }

    // GET: DATSTUDENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var datstudent = await _context.DATStudents.FindAsync(id);
        if (datstudent == null)
        {
            return NotFound();
        }
        return View(datstudent);
    }

    // POST: DATSTUDENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,DATName,DATGender,DATBirthDay,DATEmail,DATPhone,DATActive")] DATStudent datstudent)
    {
        if (id != datstudent.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(datstudent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DATStudentExists(datstudent.Id))
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
        return View(datstudent);
    }

    // GET: DATSTUDENTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var datstudent = await _context.DATStudents
            .FirstOrDefaultAsync(m => m.Id == id);
        if (datstudent == null)
        {
            return NotFound();
        }

        return View(datstudent);
    }

    // POST: DATSTUDENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var datstudent = await _context.DATStudents.FindAsync(id);
        if (datstudent != null)
        {
            _context.DATStudents.Remove(datstudent);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DATStudentExists(int? id)
    {
        return _context.DATStudents.Any(e => e.Id == id);
    }
}
