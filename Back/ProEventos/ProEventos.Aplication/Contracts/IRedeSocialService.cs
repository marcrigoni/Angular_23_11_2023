using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Aplication.Dtos;

namespace ProEventos.Aplication.Contracts
{
    public interface IRedeSocialService
    {
       Task<RedeSocialDto[]> SaveByEvento(int id, RedeSocialDto[] models);
       Task<bool> DeleteByEvento(int eventoId, int redeSocialId);

       Task<RedeSocialDto[]> SaveByPalestrante(int id, RedeSocialDto[] models);

       Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId);

       Task<RedeSocialDto[]> GetAllByEventoIdAsync(int idEvento);
       Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId);

       Task<RedeSocialDto> GetRedeSocialEventoByIdAsync(int eventoId, int redeSocialId);
       Task<RedeSocialDto> GetRedeSocialPalestranteByIdAsync(int palestranteId, int redeSocialId);
    }
}