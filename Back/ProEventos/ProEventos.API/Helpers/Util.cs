using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProEventos.API.Helpers
{
    public class Util : IUtil
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Util(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        
        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile, string destino)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imgPath = Path.Combine(_webHostEnvironment.ContentRootPath, @$"Resources/{destino}", imageName);

            using (var fileStream = new FileStream(imgPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;            
        }

        [NonAction]
        public void DeleteImage(string imageName, string destino)
        {
            if (!string.IsNullOrEmpty(imageName))
            {                
                var imgPath = Path.Combine(_webHostEnvironment.ContentRootPath, @$"Resources\{destino}", imageName);

                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }            
            }
        }
    }
}