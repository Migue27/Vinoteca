using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vinoteca.Data;
using Vinoteca.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Vinoteca.Controllers
{
    public class VinosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VinosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Vinos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vinos.Include(v => v.DenominacionOrigen);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vinos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vino = await _context.Vinos
                .Include(v => v.DenominacionOrigen)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vino == null)
            {
                return NotFound();
            }

            return View(vino);
        }

        //GET: Vinos/User
        [HttpGet]
        [Authorize(Roles="Bodeguero")]
        public async Task<IActionResult> IndexUser()
        {
            var user = _userManager.GetUserId(User);
            var applicationDbContext = _context.Vinos.Where(v => v.ApplicationUserId == user).Include(v => v.DenominacionOrigen);
               

            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Vinos/Create
        [HttpGet]
        [Authorize(Roles = "User")]
        [Authorize(Roles = "Bodeguero")]
        public IActionResult Create()
        {
            //ViewData["DenominacionOrigenId"] = new SelectList(_context.Origenes, "ID", "Denominacion");
            ViewData["DenominacionOrigenId"] = new SelectList(_context.Origenes.OrderBy(s=>s.Denominacion), "ID", "Denominacion");
            return View();
        }

        // POST: Vinos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        [Authorize(Roles = "Bodeguero")]
        public async Task<IActionResult> Create([Bind("ID,Nombre,DenominacionOrigenId,TipoVino,Anada,EdadVino,Alcohol,Color,Azucar,Unidades,Precio,Descripcion,ApplicationUserId")] Vino vino)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            vino.ApplicationUserId = user.Id;
            if (ModelState.IsValid)
            {
                _context.Add(vino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DenominacionOrigenId"] = new SelectList(_context.Origenes, "ID", "Denominacion", vino.DenominacionOrigenId);
            return View(vino);
        }

        // GET: Vinos/Edit/5
        [Authorize(Roles = "User")]
        [Authorize(Roles = "Bodeguero")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vino = await _context.Vinos.FindAsync(id);
            if (vino == null)
            {
                return NotFound();
            }
            ViewData["DenominacionOrigenId"] = new SelectList(_context.Origenes, "ID", "Denominacion", vino.DenominacionOrigenId);
            return View(vino);
        }

        // POST: Vinos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        [Authorize(Roles = "Bodeguero")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,DenominacionOrigenId,TipoVino,Anada,EdadVino,Alcohol,Color,Azucar,Unidades,Precio,Descripcion,ApplicationUserId")] Vino vino)
        {
            if (id != vino.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vino);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VinoExists(vino.ID))
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
            ViewData["DenominacionOrigenId"] = new SelectList(_context.Origenes, "ID", "Denominacion", vino.DenominacionOrigenId);
            return View(vino);
        }

        // GET: Vinos/Delete/5
        [Authorize(Roles = "User")]
        [Authorize(Roles = "Bodeguero")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vino = await _context.Vinos
                .Include(v => v.DenominacionOrigen)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vino == null)
            {
                return NotFound();
            }

            return View(vino);
        }

        // POST: Vinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        [Authorize(Roles = "Bodeguero")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vino = await _context.Vinos.FindAsync(id);
            _context.Vinos.Remove(vino);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VinoExists(int id)
        {
            return _context.Vinos.Any(e => e.ID == id);
        }
    }
}
