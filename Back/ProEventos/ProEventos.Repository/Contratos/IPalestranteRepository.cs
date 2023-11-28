using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Repository.Contratos
{
    public interface IPalestranteRepository
    {
        //Palestrantes

        Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos);

        Task<Palestrante[]> GetAllPalestrantesAsync( bool includeEventos);

        Task<Palestrante> GetPalestrantesByIdAsync(int palestranteId, bool includeEventos);

        
    }
}