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
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
// using ProEventos.API.Data;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : Controller
    {   
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventosService _eventosService;
        private readonly IPalestranteService _palestranteService;

        public RedesSociaisController(IRedeSocialService redeSocialService,
                                    IEventosService eventosService,
                                    IPalestranteService palestranteService
        )
        {
            _redeSocialService = redeSocialService;
            _eventosService = eventosService;
            _palestranteService = palestranteService;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId) 
        {
            try
            {

                if (!(await AutorEvento(eventoId)))
                {
                    return Unauthorized();
                }

                var redeSocial = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
                if (redeSocial == null)
                {
                    return NoContent();
                }

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar redeSocial. Erro: {ex.Message}");
            }
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {

                var palestrante = await _palestranteService.GetPalestrantesByUserIdAsync(User.GetUserId(), false);
                if (palestrante == null)
                {
                    return Unauthorized();
                }

                var redeSocial = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
                if (redeSocial == null)
                {
                    return NoContent();
                }

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar redeSocial. Erro: {ex.Message}");
            }
        }        

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {
            var evento = await _eventosService.GetEventosByIdAsync(User.GetUserId(), eventoId, false);
            if (evento == null)
            {
                return false;
            }

            return true;
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            // try
            // {
            //     var evento = await _loteService.GetAllEventosByTemaAsync(tema, true);

            //     if (evento == null)
            //     {
            //         return NoContent();
            //     }
            //     return Ok(evento);
            // }
            // catch (System.Exception ex)
            // {
            //     return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
            // }
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            // try
            // {
            //     var evento = await _loteService.AddEventos(model);

            //     if (evento == null)
            //     {
            //         return NoContent();
            //     }
            //     return Ok(evento);
            // }
            // catch (System.Exception ex)
            // {
            //     return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
            // }
            throw new NotImplementedException();
        }

        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                if (!(await AutorEvento(eventoId)))
                {
                    return Unauthorized();
                }

                var redeSocial = await _redeSocialService.SaveByEvento(eventoId, models);

                if (redeSocial == null)
                {
                    return NoContent();
                }
                return Ok(redeSocial);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
            }
        }

        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestrantesByUserIdAsync(User.GetUserId(), false);
                if (palestrante == null)
                {
                    return Unauthorized();
                }                                

                var redeSocial = await _redeSocialService.SaveByPalestrante(palestrante.Id, models);

                if (redeSocial == null)
                {
                    return NoContent();
                }

                return Ok(redeSocial);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar redes sociais. Erro: {ex.Message}");
            }
        }



        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialService.GetRedeSocialEventoByIdAsync(eventoId, redeSocialId);
                if (redeSocial == null)
                {
                    return NoContent();
                }

                if (await _redeSocialService.DeleteByEvento(eventoId, redeSocial.Id))
                {
                    return Ok(new {message = "redeSocial Deletado!" });
                }
                else
                {
                    throw new Exception("Ocorreu um problema não específico ao deletar a rede social");
                }                
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestrantesByUserIdAsync(User.GetUserId(), false);
                if (palestrante == null)
                {
                    return Unauthorized();
                }

                var redeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdAsync(palestrante.Id, redeSocialId);
                if (redeSocial == null)
                {
                    return NoContent();
                }

                if (await _redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocialId))
                {
                    return Ok(new { message = "RedeSocial Deletado!" });
                }
                else
                {
                    throw new Exception("Ocorreu um problema não específico ao deletar a rede social");
                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar lotes. Erro: {ex.Message}");
            }
        }


    }
}