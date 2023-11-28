using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Repository.Contratos
{
    public interface IEventosRepository
{
    Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);

    Task<Evento[]> GetAllEventosAsync(bool includePalestrantes);

    Task<Evento> GetEventosByIdAsync(int eventoId, bool includePalestrantes);
}
}