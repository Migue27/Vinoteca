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
using Vinoteca.ViewModels;
using Microsoft.AspNetCore.Hosting;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Vinoteca.Controllers
{
    public class VinosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ImagesController _imagesController;

        private readonly IWebHostEnvironment _env;

        public VinosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ImagesController imagesController, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _imagesController = imagesController;
            _env = env;

        }

        // GET: Vinos
        public async Task<IActionResult> Index()
        {
            
            var applicationDbContext = await _context.Vinos.Include(v => v.DenominacionOrigen).ToListAsync();

            applicationDbContext.ForEach(m => m.VinoPath = "~/images/"+ m.VinoPath);
            return View(applicationDbContext);

            //var vinos = new List <Vino>();
            //foreach(var vino in applicationDbContext)
            //{
            //    var vinoView = new Vino();
            //    //TODO: Imagen y resto de props
            //    vinoView.ID = vino.ID;
            //    vinoView.Nombre = vino.Nombre;
            //    vinoView.DenominacionOrigen = vino.DenominacionOrigen;
            //    vinoView.DenominacionOrigenId = vino.DenominacionOrigenId;
            //    vinoView.TipoVino = vino.TipoVino;
            //    vinoView.Anada = vino.Anada;
            //    vinoView.EdadVino = vino.EdadVino;
            //    vinoView.Alcohol = vino.Alcohol;
            //    vinoView.Color = vino.Color;
            //    vinoView.Azucar = vino.Azucar;
            //    vinoView.Unidades = vino.Unidades;
            //    vinoView.Precio = vino.Precio;
            //    vinoView.Descripcion = vino.Descripcion;
            //    vinoView.ApplicationUser = vino.ApplicationUser;
            //    vinoView.ApplicationUserId = vino.ApplicationUserId;

            //    //TO SET IMAGE

            //    //var result = await _imagesController.GetImage(vino.VinoPath);
            //    //var stream = result.FileStream;
            //    //var memoryStream = new MemoryStream();
            //    //await stream.CopyToAsync(memoryStream);
            //    //memoryStream.Position = 0;
            //    //var fileName = Path.GetFileName(vino.VinoPath);
            //    //var file = new FormFile(memoryStream, 0, memoryStream.Length, fileName, fileName);

            //    //vinoView.VinoImage = await _imagesController.GetImage(vino.VinoPath);
            //    vinoView.VinoImage = Path.Combine(_env.WebRootPath, "images", vino.VinoPath.Substring(1));
            //    vinos.Add(vinoView);

            //}

            //return View(vinos);
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
        public async Task<IActionResult> Create([Bind("ID,Nombre,DenominacionOrigenId,TipoVino,Anada,EdadVino,Alcohol,Color,Azucar,Unidades,Precio,Descripcion,ApplicationUserId,VinoImage")] VinoViewModel vino)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            vino.ApplicationUserId = user.Id;

            if (ModelState.IsValid)
            {
                string imagePath = "";
                //if (vino.VinoImage != null)
                //{
                    IActionResult resp = await _imagesController.UploadProductImage(vino.VinoImage, user, vino);

                if(resp is BadRequestObjectResult bad)
                {
                    ModelState.Merge(bad.Value as ModelStateDictionary);
                    var errors = (SerializableError)bad.Value;
                    //var errorMessage = errors.FirstOrDefault().Value.ToString();
                    //ModelState.AddModelError("VinoImage", errorMessage);
                    ModelState.AddModelError("VinoImage", ((string[])errors.First().Value)[0]);

                    ViewData["DenominacionOrigenId"] = new SelectList(_context.Origenes, "ID", "Denominacion", vino.DenominacionOrigenId);
                    return View(vino);
                }

                if (resp is OkObjectResult result)
                    {
                        imagePath = result.Value as string;
                    }
                //}
                Vino vinoModel = new Vino();

                vinoModel.ID = vino.ID;
                vinoModel.Nombre = vino.Nombre;
                vinoModel.DenominacionOrigen = vino.DenominacionOrigen;
                vinoModel.DenominacionOrigenId = vino.DenominacionOrigenId;
                vinoModel.TipoVino = vino.TipoVino;
                vinoModel.Anada = vino.Anada;
                vinoModel.EdadVino = vino.EdadVino;
                vinoModel.Alcohol=vino.Alcohol;
                vinoModel.Color=vino.Color;
                vinoModel.Azucar=vino.Azucar;
                vinoModel.Unidades=vino.Unidades;
                vinoModel.Precio=vino.Precio;
                vinoModel.Descripcion=vino.Descripcion;
                vinoModel.ApplicationUser=vino.ApplicationUser;
                vinoModel.ApplicationUserId=vino.ApplicationUserId;

                vinoModel.VinoPath = imagePath;



                _context.Add(vinoModel);
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
            VinoViewModel vinoView = new VinoViewModel();

            vinoView.ID = vino.ID;
            vinoView.Nombre = vino.Nombre;
            vinoView.DenominacionOrigen = vino.DenominacionOrigen;
            vinoView.DenominacionOrigenId = vino.DenominacionOrigenId;
            vinoView.TipoVino = vino.TipoVino;
            vinoView.Anada = vino.Anada;
            vinoView.EdadVino = vino.EdadVino;
            vinoView.Alcohol = vino.Alcohol;
            vinoView.Color = vino.Color;
            vinoView.Azucar = vino.Azucar;
            vinoView.Unidades = vino.Unidades;
            vinoView.Precio = vino.Precio;
            vinoView.Descripcion = vino.Descripcion;
            vinoView.ApplicationUser = vino.ApplicationUser;
            vinoView.ApplicationUserId = vino.ApplicationUserId;

            vinoView.VinoPath = vino.VinoPath;

            ViewData["DenominacionOrigenId"] = new SelectList(_context.Origenes, "ID", "Denominacion", vino.DenominacionOrigenId);
            return View(vinoView);
        }

        // POST: Vinos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        [Authorize(Roles = "Bodeguero")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,DenominacionOrigenId,TipoVino,Anada,EdadVino,Alcohol,Color,Azucar,Unidades,Precio,Descripcion,ApplicationUserId,VinoImage,VinoPath")] VinoViewModel vino)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            vino.ApplicationUserId = user.Id;

            if (id != vino.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {


                    var vinoModel = await _context.Vinos.FindAsync(vino.ID);
                    if(vinoModel == null)
                    {
                        return NotFound();
                    }

                    vinoModel.ID = vino.ID;
                    vinoModel.Nombre = vino.Nombre;
                    vinoModel.DenominacionOrigen = vino.DenominacionOrigen;
                    vinoModel.DenominacionOrigenId = vino.DenominacionOrigenId;
                    vinoModel.TipoVino = vino.TipoVino;
                    vinoModel.Anada = vino.Anada;
                    vinoModel.EdadVino = vino.EdadVino;
                    vinoModel.Alcohol = vino.Alcohol;
                    vinoModel.Color = vino.Color;
                    vinoModel.Azucar = vino.Azucar;
                    vinoModel.Unidades = vino.Unidades;
                    vinoModel.Precio = vino.Precio;
                    vinoModel.Descripcion = vino.Descripcion;
                    //vinoModel.ApplicationUser = vino.ApplicationUser;
                    vinoModel.ApplicationUserId = vino.ApplicationUserId;

                    //TO SET NEW IMAGE
                    
                    if (vino.VinoImage != null)
                    {
                        string imagePath = "";

                        IActionResult resp = await _imagesController.UploadProductImage(vino.VinoImage, user, vino);

                        if (resp is BadRequestObjectResult bad)
                        {
                            ModelState.Merge(bad.Value as ModelStateDictionary);
                            var errors = (SerializableError)bad.Value;

                            ModelState.AddModelError("VinoImage", ((string[])errors.First().Value)[0]);

                            ViewData["DenominacionOrigenId"] = new SelectList(_context.Origenes, "ID", "Denominacion", vino.DenominacionOrigenId);
                            return View(vino);
                        }

                        if (resp is OkObjectResult result)
                        {
                            imagePath = result.Value as string;
                        }

                        vinoModel.VinoPath = imagePath;
                    }
                  


                    _context.Update(vinoModel);
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
