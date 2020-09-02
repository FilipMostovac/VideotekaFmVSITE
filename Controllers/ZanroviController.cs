using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideotekaFm.Models;

namespace VideotekaFm.Controllers
{
    [Authorize]
    public class ZanroviController : Controller
    {
        private readonly VideotekaContext _context;

        public ZanroviController(VideotekaContext context)
        {
            _context = context;
        }

        // GET: Zanrovi
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zanr.ToListAsync());
        }

        // GET: Zanrovi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zanr = await _context.Zanr
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zanr == null)
            {
                return NotFound();
            }

            zanr.Filmovi = await _context.Filmovi  // SELECT ... FROM dbo.Filmovi WHERE ID = ... ORDER BY Naziv
                .Where(film => film.ZanrId == id)
                .OrderBy(film => film.Naziv)
                .ToListAsync();

            return View(zanr);
        }

        // GET: Zanrovi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zanrovi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv")] Zanr zanr)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zanr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zanr);
        }

        // GET: Zanrovi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zanr = await _context.Zanr.FindAsync(id);
            if (zanr == null)
            {
                return NotFound();
            }
            return View(zanr);
        }

        // POST: Zanrovi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv")] Zanr zanr)
        {
            if (id != zanr.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zanr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZanrExists(zanr.Id))
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
            return View(zanr);
        }

        // GET: Zanrovi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zanr = await _context.Zanr
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zanr == null)
            {
                return NotFound();
            }

            return View(zanr);
        }

        // POST: Zanrovi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zanr = await _context.Zanr.FindAsync(id);
            _context.Zanr.Remove(zanr);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZanrExists(int id)
        {
            return _context.Zanr.Any(e => e.Id == id);
        }
    }
}
