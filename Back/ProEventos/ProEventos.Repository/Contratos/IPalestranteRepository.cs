using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;
using ProEventos.Repository.Models;

namespace ProEventos.Repository.Contratos
{
    public interface IPalestranteRepository : IGeralRepository
    {
        Task<PageList<Palestrante>> GetAllPalestrantesAsync( PageParams pageParams, bool includeEventos = false );

        Task<Palestrante> GetPalestrantesByUserIdAsync(int userId, bool includeEventos = false);        
    }
}