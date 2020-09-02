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
    public class RezervacijeController : Controller
    {
        private readonly VideotekaContext _context;

        public RezervacijeController(VideotekaContext context)
        {
            _context = context;
        }

        // GET: Rezervacije
        public async Task<IActionResult> Index()
        {
            var videotekaContext = _context.Rezervacije.Include(r => r.Clan).Include(r => r.Film);
            return View(await videotekaContext.ToListAsync());
        }

        // GET: Rezervacije/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Clan)
                .Include(r => r.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // GET: Rezervacije/Create
        public IActionResult Create()
        {
            ViewData["ClanId"] = new SelectList(_context.Clanovi, "Id", "PrezimeIme");
            ViewData["FilmId"] = new SelectList(_context.Filmovi, "Id", "Naziv");
            return View();
        }

        // POST: Rezervacije/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClanId,FilmId,PocetakPosudbe,Vraceno")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(rezervacija);
                _context.Entry(rezervacija)
                    .Reference(r => r.Film)
                    .Load();

                if (rezervacija.Film.Kolicina < 1)
                {
                    return View("Error", new ErrorViewModel { ErrorMessage = "Ponestalo filmova :(" });
                }
                else
                {
                    rezervacija.Film.Kolicina--;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClanId"] = new SelectList(_context.Clanovi, "Id", "PrezimeIme", rezervacija.ClanId);
            ViewData["FilmId"] = new SelectList(_context.Filmovi, "Id", "Naziv", rezervacija.FilmId);
            return View(rezervacija);
        }

        // GET: Rezervacije/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            ViewData["ClanId"] = new SelectList(_context.Clanovi, "Id", "PrezimeIme", rezervacija.ClanId);
            ViewData["FilmId"] = new SelectList(_context.Filmovi, "Id", "Naziv", rezervacija.FilmId);
            return View(rezervacija);
        }

        // POST: Rezervacije/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClanId,FilmId,PocetakPosudbe,Vraceno, Naziv")] Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);

                    _context.Entry(rezervacija)
                        .Reference(r => r.Film)
                        .Load();

                    var vraceno = _context.Entry(rezervacija)
                        .Property(r => r.Vraceno)
                        .IsModified;

                    if (vraceno)
                    {
                        rezervacija.Film.Kolicina++;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.Id))
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
            ViewData["ClanId"] = new SelectList(_context.Clanovi, "Id", "PrezimeIme", rezervacija.ClanId);
            ViewData["FilmId"] = new SelectList(_context.Filmovi, "Id", "Naziv", rezervacija.FilmId);
            return View(rezervacija);
        }

        // GET: Rezervacije/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Clan)
                .Include(r => r.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // POST: Rezervacije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            _context.Rezervacije.Remove(rezervacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacije.Any(e => e.Id == id);
        }
    }
}
