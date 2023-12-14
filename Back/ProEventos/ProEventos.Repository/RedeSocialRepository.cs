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
    public class RedeSocialRepository : GeralRepository, IRedeSocialRepository
    {
        private readonly ProEventosContext _proEventosContext;
        public RedeSocialRepository(ProEventosContext proEventosContext) : base(proEventosContext)
        {
            _proEventosContext = proEventosContext;
        }

        public async Task<RedeSocial> GetRedeSocialEventoByIdAsync(int eventoId, int id){            
            IQueryable<RedeSocial> query = _proEventosContext.RedesSociais;

            query = query.AsNoTracking().Where(rs => rs.EventoId == eventoId && 
                            rs.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<RedeSocial> GetRedeSocialPalestranteByIdAsync(int palestranteId, int id)
        {
            IQueryable<RedeSocial> query = _proEventosContext.RedesSociais;

            query = query.AsNoTracking().Where(rs => rs.PalestranteId == palestranteId &&
                            rs.Id == id);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId)
        {
            IQueryable<RedeSocial> query = _proEventosContext.RedesSociais;

            query = query.AsNoTracking().Where(rs => rs.EventoId == eventoId);

            return await query.ToArrayAsync();
            
        }
        public async Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId)
        {   
            IQueryable<RedeSocial> query = _proEventosContext.RedesSociais;           

            query = query.AsNoTracking().Where(rs => rs.PalestranteId == palestranteId);

            return await query.ToArrayAsync();
        }
    }
}