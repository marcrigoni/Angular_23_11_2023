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
    public class PalestrantesRepository : IPalestranteRepository
    {
        private readonly ProEventosContext _context;

        public PalestrantesRepository(ProEventosContext context)
        {
            _context = context;
        }


        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(e => e.Eventos);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id).Where(
                e => e.User.PrimeiroNome.ToLower().Contains(nome.ToLower()) &&
                e.User.UltimoNome.ToLower().Contains(nome.ToLower()));
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(e => e.Eventos);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id);
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestrantesByIdAsync(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(e => e.Eventos);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id).Where(e => e.Id == palestranteId);
            return await query.FirstOrDefaultAsync();
        }
    }
}