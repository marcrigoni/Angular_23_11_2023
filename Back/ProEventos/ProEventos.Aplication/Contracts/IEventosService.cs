using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Aplication.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.Aplication.Contracts
{
    public interface IEventosService
    {
        Task<EventoDto> AddEventos(EventoDto model);

        Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);

        Task<bool> DeleteEvento(int eventoId);


        Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false );

        Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false);

        Task<EventoDto> GetEventosByIdAsync(int eventoId, bool includePalestrantes = false);
    }
}