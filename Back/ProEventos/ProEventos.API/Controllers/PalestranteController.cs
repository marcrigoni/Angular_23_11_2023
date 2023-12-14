using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Aplication.Dtos;
using ProEventos.Aplication.Contracts;
using ProEventos.Repository;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Repository.Models;
// using ProEventos.API.Data;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestranteController : Controller
    {   
        private readonly IPalestranteService _palestranteService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;

        public PalestranteController(IPalestranteService palestranteService, 
                                IWebHostEnvironment webHostEnvironment,
                                IAccountService accountService)
        {
            _accountService = accountService;
            _webHostEnvironment = webHostEnvironment;
            _palestranteService = palestranteService;            
        }

        [HttpGet("all") ]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams) 
        {
            try
            {
                var palestrantes = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);
                if (palestrantes == null)
                {
                    return NoContent();
                }

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);

                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPalestrantes()
        {
            try
            {
                var evento = await _palestranteService.GetPalestrantesByUserIdAsync(User.GetUserId());

                if (evento == null)
                {
                    return NoContent();
                }
                return Ok(evento);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }            
        }        

        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestrantesByUserIdAsync(User.GetUserId(), false);

                if(palestrante == null)
                {
                    palestrante = await _palestranteService.AddPalestrantes(User.GetUserId(), model);
                    return Ok(palestrante);
                }

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar palestrante. Erro: {ex.Message}");
            }
        }
        
        [NonAction]
        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imgPath = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources/images", imageName);

            using (var fileStream = new FileStream(imgPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            
            return imageName;
        }

        [NonAction]
        private void DeleteImage(string imageName)
        {
            var imgPath = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources/images", imageName);

            if (System.IO.File.Exists(imgPath))
            {
                System.IO.File.Delete(imgPath);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestranteService.UpdatePalestrante(User.GetUserId(), model);

                if (palestrante == null)
                {
                    return NoContent();
                }
                return Ok(palestrante);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar palestrante. Erro: {ex.Message}");
            }
        }
    }
}