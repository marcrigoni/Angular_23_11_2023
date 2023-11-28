using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Aplication.Contracts
{
    public interface IEventosService
    {
        Task<Evento> AddEventos(Evento model);

        Task<Evento> UpdateEvento(int eventoId, Evento model);

        Task<bool> DeleteEvento(int eventoId);


        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false );

        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);

        Task<Evento> GetEventosByIdAsync(int eventoId, bool includePalestrantes = false);
    }
}