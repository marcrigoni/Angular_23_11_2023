using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Repository.Contratos
{
    public interface IEventosRepository
    {
        Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);

        Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);

        Task<Evento> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}