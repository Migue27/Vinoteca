using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vinoteca.Data;
using Vinoteca.Models;

namespace Vinoteca.Controllers
{
    public class DenominacionOrigensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DenominacionOrigensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DenominacionOrigens
        public async Task<IActionResult> Index()
        {
            return View(await _context.Origenes.ToListAsync());
        }

        // GET: DenominacionOrigens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var denominacionOrigen = await _context.Origenes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (denominacionOrigen == null)
            {
                return NotFound();
            }

            return View();
        }

        //GET: DenominacionOrigens/Create
        public IActionResult Create()
        {
            return View();
        }

        //[HttpGet]
        //public PartialViewResult Create()
        //{
            
        //    return PartialView("~/views/Shared/_PartialCreateDenominacion.cshtml");
        //}
        // POST: DenominacionOrigens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Denominacion")] DenominacionOrigen denominacionOrigen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(denominacionOrigen);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Vinos");
                //return View("~/Views/Vinos/Create.cshtml");
            }
            return View(denominacionOrigen);
        }

        //// GET: DenominacionOrigens/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var denominacionOrigen = await _context.Origenes.FindAsync(id);
        //    if (denominacionOrigen == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(denominacionOrigen);
        //}

        //// POST: DenominacionOrigens/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,Denominacion")] DenominacionOrigen denominacionOrigen)
        //{
        //    if (id != denominacionOrigen.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(denominacionOrigen);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DenominacionOrigenExists(denominacionOrigen.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(denominacionOrigen);
        //}

        //// GET: DenominacionOrigens/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var denominacionOrigen = await _context.Origenes
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (denominacionOrigen == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(denominacionOrigen);
        //}

        //// POST: DenominacionOrigens/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var denominacionOrigen = await _context.Origenes.FindAsync(id);
        //    _context.Origenes.Remove(denominacionOrigen);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool DenominacionOrigenExists(int id)
        {
            return _context.Origenes.Any(e => e.ID == id);
        }
    }
}
