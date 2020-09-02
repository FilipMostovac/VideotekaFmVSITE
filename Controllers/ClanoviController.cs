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
    public class ClanoviController : Controller
    {
        private readonly VideotekaContext _context;

        public ClanoviController(VideotekaContext context)
        {
            _context = context;
        }

        // GET: Clanovi
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clanovi.ToListAsync());
        }

        // GET: Clanovi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clanovi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clan == null)
            {
                return NotFound();
            }
            clan.Rezervacije = await _context.Rezervacije  // SELECT ... FROM dbo.Filmovi WHERE ID = ... ORDER BY Naziv
                .Include(r => r.Film)
                .Where(Rezervacija => clan.Id == Rezervacija.Clan.Id)
                .ToListAsync();
            return View(clan);
        }

        // GET: Clanovi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clanovi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ime,Prezime,Email,Telefon,Adresa")] Clan clan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clan);
        }

        // GET: Clanovi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clanovi.FindAsync(id);
            if (clan == null)
            {
                return NotFound();
            }
            return View(clan);
        }

        // POST: Clanovi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Prezime,Email,Telefon,Adresa")] Clan clan)
        {
            if (id != clan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClanExists(clan.Id))
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
            return View(clan);
        }

        // GET: Clanovi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clanovi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clan == null)
            {
                return NotFound();
            }

            return View(clan);
        }

        // POST: Clanovi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clan = await _context.Clanovi.FindAsync(id);
            _context.Clanovi.Remove(clan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClanExists(int id)
        {
            return _context.Clanovi.Any(e => e.Id == id);
        }
    }
}
