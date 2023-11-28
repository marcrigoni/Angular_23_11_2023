using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Repository.Context;
using ProEventos.Repository.Contratos;

namespace ProEventos.Repository
{
    public class EventosRepository : IEventosRepository
    {
        private readonly ProEventosContext _context;

        public EventosRepository(ProEventosContext context)
        {
            _context = context;            
        }        
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos.Include(
                e => e.Lotes
            ).Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            query = query.OrderBy(e => e.Id);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos.Include(
                e => e.Lotes
            ).Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            query = query.OrderBy(e => e.Id).AsNoTracking().Where(e => e.Tema.ToLower().Contains(tema.ToLower()));
            return await query.ToArrayAsync();            
        }        

        public async Task<Evento> GetEventosByIdAsync(int eventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos.Include(
                e => e.Lotes
            ).Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);
            }

            query = query.OrderBy(e => e.Id).AsNoTracking().Where(e => e.Id == eventoId);
            return await query.FirstOrDefaultAsync();
        }
    }
}