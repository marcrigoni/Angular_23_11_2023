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
using ProEventos.API.Helpers;
// using ProEventos.API.Data;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : Controller
    {   
        private readonly IEventosService _service;
        private readonly IUtil _util;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;

        private readonly string destino = "Images";

        public EventosController(IEventosService service, 
                                IUtil util,
                                IAccountService accountService)
        {
            _accountService = accountService;            
            _service = service;
            _util = util;
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
                    _util.DeleteImage(evento.ImageURL, destino);
                    evento.ImageURL = await _util.SaveImage(file, destino);
                }
                var eventoRetorno = await _service.UpdateEvento(User.GetUserId(),  eventoId, evento);

                return Ok(eventoRetorno);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
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