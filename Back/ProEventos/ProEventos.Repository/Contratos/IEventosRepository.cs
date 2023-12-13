using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;
using ProEventos.Repository.Models;

namespace ProEventos.Repository.Contratos
{
    public interface IEventosRepository
    {
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);

        Task<Evento> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}