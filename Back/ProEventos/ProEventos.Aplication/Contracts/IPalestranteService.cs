using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Aplication.Dtos;
using ProEventos.Domain.Models;
using ProEventos.Repository.Models;

namespace ProEventos.Aplication.Contracts
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model);
        
        Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model);                

        Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams,  bool includeEventos = false);        

        Task<PalestranteDto> GetPalestrantesByUserIdAsync(int userId, bool includeEventos = false);
    }
}