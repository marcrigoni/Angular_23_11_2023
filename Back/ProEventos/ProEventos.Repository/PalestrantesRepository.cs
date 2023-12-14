using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Repository.Context;
using ProEventos.Repository.Contratos;
using ProEventos.Repository.Models;

namespace ProEventos.Repository
{
    public class PalestrantesRepository : GeralRepository, IPalestranteRepository
    {
        private readonly ProEventosContext _context;

        public PalestrantesRepository(ProEventosContext context) : base(context)
        {
            _context = context;
        }           

        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams,  bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.
                Include(p => p.User).
                Include(e => e.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(e => e.Eventos);
            }

            query = query.AsNoTracking()
            .Where(p => (p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                        p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                        p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower())) &&
                        p.User.Funcao == Domain.Enum.Funcao.Palestrante).OrderBy(p => p.Id);

            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Palestrante> GetPalestrantesByUserIdAsync(int userId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.
                Include(e => e.RedesSociais).
                Include(e => e.User);

            if (includeEventos)
            {
                query = query.Include(e => e.Eventos);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id).Where(e => e.UserId == userId);
            
            return await query.FirstOrDefaultAsync();
        }
    }
}