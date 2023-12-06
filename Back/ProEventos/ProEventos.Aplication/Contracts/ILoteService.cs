using System.Threading.Tasks;
using ProEventos.Aplication.Dtos;

namespace ProEventos.Aplication.Contracts
{
    public interface ILoteService
    {
        Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models);

        Task<bool> DeleteLote(int eventoId, int loteId);


        Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);        

        Task<LoteDto> GetLoteByIdAsync(int eventoId, int loteId);
    }
}