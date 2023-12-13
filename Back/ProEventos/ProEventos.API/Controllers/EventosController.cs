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
    public class EventosController : Controller
    {   
        private readonly IEventosService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;

        public EventosController(IEventosService service, 
                                IWebHostEnvironment webHostEnvironment,
                                IAccountService accountService)
        {
            _accountService = accountService;
            _webHostEnvironment = webHostEnvironment;
            _service = service;            
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams) 
        {
            try
            {
                var eventos = await _service.GetAllEventosAsync(User.GetUserId(), pageParams, true);
                if (eventos == null)
                {
                    return NoContent();
                }

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _service.GetEventosByIdAsync(User.GetUserId(), id, true);

                if (evento == null)
                {
                    return NoContent();
                }
                return Ok(evento);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
            }            
        }        

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                // var evento = await _service.UpdateEvento()
                var evento = await _service.GetEventosByIdAsync(User.GetUserId(), eventoId, true);
                if (evento == null)
                {
                    return NoContent();
                }

                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    DeleteImage(evento.ImageURL);
                    evento.ImageURL = await SaveImage(file);
                }
                var eventoRetorno = await _service.UpdateEvento(User.GetUserId(),  eventoId, evento);

                return Ok(eventoRetorno);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
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

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _service.AddEventos(User.GetUserId(), model);

                if (evento == null)
                {
                    return NoContent();
                }
                return Ok(evento);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _service.UpdateEvento(User.GetUserId(), id, model);

                if (evento == null)
                {
                    return NoContent();
                }
                return Ok(evento);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _service.GetEventosByIdAsync(User.GetUserId(),  id, true);
                if (evento == null)
                {
                    return NoContent();
                }

                if (await _service.DeleteEvento(User.GetUserId(), id))
                {
                    DeleteImage(evento.ImageURL);
                    return Ok(new {message = "Deletado!" });
                }
                else
                {
                    throw new Exception("Ocorreu um problema não específico ao deletar evento");
                }                
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
            }
        }

        
    }
}