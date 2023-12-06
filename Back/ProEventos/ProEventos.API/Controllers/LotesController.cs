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
// using ProEventos.API.Data;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : Controller
    {   
        private readonly ILoteService _loteService;
        private readonly IEventosService _eventoService;

        public LotesController(ILoteService loteService, IEventosService eventosService)
        {
            _eventoService = eventosService;
            _loteService = loteService;            
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId) 
        {
            try
            {
                var lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null)
                {
                    return NoContent();
                }

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }
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

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _loteService.SaveLote(eventoId, models);

                if (lotes == null)
                {
                    return NoContent();
                }
                return Ok(lotes);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteService.GetLoteByIdAsync(eventoId, loteId);
                if (lote == null)
                {
                    return NoContent();
                }

                if (await _loteService.DeleteLote(lote.EventoId, lote.Id))
                {
                    return Ok(new {message = "Lote Deletado!" });
                }
                else
                {
                    throw new Exception("Ocorreu um problema não específico ao deletar o lote");
                }                
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar lotes. Erro: {ex.Message}");
            }
        }
    }
}