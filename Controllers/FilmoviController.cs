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
    public class FilmoviController : Controller
    {
        private readonly VideotekaContext _context;

        public FilmoviController(VideotekaContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            var videotekaContext = _context.Filmovi
                    .Include(f => f.Zanr);
            return View(await videotekaContext.ToListAsync());
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Filmovi
                .Include(f => f.Zanr)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }
            film.Rezervacije = await _context.Rezervacije  // SELECT ... FROM dbo.Filmovi WHERE ID = ... ORDER BY Naziv
                .Include(r => r.Clan)
                .Where(Rezervacija => film.Id == Rezervacija.Film.Id)
                .ToListAsync();
            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            ViewData["ZanrId"] = new SelectList(_context.Zanr, "Id", "Naziv");
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Trajanje,Kolicina,ZanrId,ImgPath")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ZanrId"] = new SelectList(_context.Zanr, "Id", "Naziv", film.ZanrId);
            return View(film);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Filmovi.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            ViewData["ZanrId"] = new SelectList(_context.Zanr, "Id", "Naziv", film.ZanrId);
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Trajanje,Kolicina,ZanrId,ImgPath")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
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
            ViewData["ZanrId"] = new SelectList(_context.Zanr, "Id", "Id", film.ZanrId);
            return View(film);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Filmovi
                .Include(f => f.Zanr)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Filmovi.FindAsync(id);
            _context.Filmovi.Remove(film);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Filmovi.Any(e => e.Id == id);
        }
    }
}
