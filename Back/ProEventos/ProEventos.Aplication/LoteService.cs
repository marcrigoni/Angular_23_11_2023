using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Aplication.Contracts;
using ProEventos.Aplication.Dtos;
using ProEventos.Domain.Models;
using ProEventos.Repository.Contratos;

namespace ProEventos.Aplication
{
    public class LoteService : ILoteService
    {
        private readonly IGeralRepository _geralRepository;
        private readonly ILoteRepository _lotesRepository;
        private readonly IMapper _mapper;

        public LoteService(IGeralRepository geralRepository, 
                                ILoteRepository loteRepository,
                                IMapper mapper)
        {
            _mapper = mapper;
            _lotesRepository = loteRepository;
            _geralRepository = geralRepository;            
        }

        public async Task AddLote(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);

                lote.EventoId = eventoId;

                _geralRepository.Add<Lote>(lote);

                await _geralRepository.SaveChangesAsync();                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotesRepository.GetLoteByIdAsync(eventoId, loteId);
                if (lote == null)
                {
                    throw new Exception("Lote para deleção não encontrado");
                }

                _geralRepository.Delete<Lote>(lote);
                return await _geralRepository.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            // try
            // {
            //     var eventos = await _eventosRepository.GetAllEventosAsync(includePalestrantes);
            //     if (eventos == null)
            //     {
            //         return null;
            //     }

            //     var resultad = _mapper.Map<EventoDto[]>(eventos);


            //     return resultad;
            // }
            // catch (System.Exception ex)
            // {
            //     throw new Exception(ex.Message);
            // }
            throw new NotImplementedException();
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotesRepository.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<LoteDto[]>(lotes);

                return resultad;
            }
            catch (System.Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotesRepository.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null)
                {
                    return null;
                }

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddLote(eventoId, model);
                    }
                    else
                    {
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);

                        model.EventoId = eventoId;

                        _mapper.Map(model, lote);

                        _geralRepository.Update<Lote>(lote);

                        await _geralRepository.SaveChangesAsync();
                    }
                }

                var loteRetorno = await _lotesRepository.GetLotesByEventoIdAsync(eventoId);

                return _mapper.Map<LoteDto[]>(loteRetorno);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public async Task<LoteDto> GetLoteByIdAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotesRepository.GetLoteByIdAsync(eventoId, loteId);
                if (lote == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<LoteDto>(lote);

                return resultad;
                
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message) ;
            }            
        }        
    }
}