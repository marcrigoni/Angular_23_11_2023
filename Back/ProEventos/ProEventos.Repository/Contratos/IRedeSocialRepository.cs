using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Repository.Contratos
{
    public interface IRedeSocialRepository : IGeralRepository
    {
        Task<RedeSocial> GetRedeSocialEventoByIdAsync(int eventoId, int redeSocialId);

        Task<RedeSocial> GetRedeSocialPalestranteByIdAsync(int palestranteId, int id);
        Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId);
    }
}