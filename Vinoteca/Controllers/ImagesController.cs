using Vinoteca.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Win32;
using System;
using Microsoft.AspNetCore.Identity;
using Vinoteca.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace Vinoteca.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public ImagesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        private static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfileImage(IFormFile image, ApplicationUser user)
        {
            
            if (image == null || !image.ContentType.Contains("image"))
            {
                // error handling
                ModelState.AddModelError("File", "El archivo no es de tipo imagen");
                return BadRequest(ModelState);
            }
            if (image.Length == 0)
            {
                ModelState.AddModelError("File", "No se ha seleccionado ninguna imagen.");
                return BadRequest(ModelState);
            }

            // verify image and save in profile path
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            //if user's directory doesn't exist create it
            var directory = Directory.GetCurrentDirectory() + "/wwwroot/images/UserProfile/" + user.Nombre;

            CreateFolder(directory);
            
            string path = Path.Combine(directory +"/", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return Ok("/UserProfile/" +user.Nombre+ "/" + fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProductImage(IFormFile image, ApplicationUser user, VinoViewModel vino)
        {

            if (image == null || !image.ContentType.Contains("image") || image.Length == 0)
            {
                // error handling
                ModelState.AddModelError("VinoImage", "No se ha seleccionado ningun archivo o no es de tipo imagen");
                return BadRequest(ModelState);
            }
            

            // verify image and save in profile path
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            //if user's directory doesn't exist create it
            var directory = Directory.GetCurrentDirectory() + "/wwwroot/images/Vinos/"  + user.Nombre + "/" + vino.Nombre;
            CreateFolder(directory);

            string path = Path.Combine(directory + "/", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Ok("/Vinos/" + user.Nombre + "/" + vino.Nombre + "/" + fileName);
        }

        public async Task<IFormFile> GetImage(string imagePath)
        {
            string path = Path.Combine(_env.WebRootPath, "images", imagePath.Substring(1));
            //string path = _env.WebRootPath + "/images"+ imagePath;

            //using is for Dispose() stream and release resource
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return await Task.FromResult(new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)));
            }


                
        }



        //private string Path { get; set; }
        //private string Name 
        //{ 
        //    get { return Name; } 
        //    set { Name = value; } 
        //}


        //public ImagesController(string path, string name)
        //{
        //    Path = RootPath + path;
        //    Name = name;
        //}

        //public bool StartStoreProcess()
        //{
        //    CreateFolder();

        //    if(StoreImage())
        //        return true;
        //    return false;
        //}

        //private void CreateFolder()
        //{
        //    if(!Directory.Exists(Path))
        //    {
        //        Directory.CreateDirectory(Path);               
        //    }

        //}
        //private bool StoreImage()
        //{

        //    return false;
        //}
    }
}
