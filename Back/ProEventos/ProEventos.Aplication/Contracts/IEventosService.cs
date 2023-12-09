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
        Task<EventoDto> AddEventos(int userId, EventoDto model);

        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);

        Task<bool> DeleteEvento(int userId, int eventoId);


        Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false );

        Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);

        Task<EventoDto> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}