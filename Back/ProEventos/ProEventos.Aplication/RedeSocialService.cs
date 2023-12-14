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
    public class RedeSocialService : IRedeSocialService
    {        
        private readonly IRedeSocialRepository _redeSocialRepository;
        private readonly IMapper _mapper;

        public RedeSocialService(IRedeSocialRepository redeSocialRepository,
                                IMapper mapper)
        {
            _redeSocialRepository = redeSocialRepository;
            _mapper = mapper;            
        }

        public async Task AddRedeSocial(int id, RedeSocialDto model, bool isEvento)
        {
            try
            {
                var RedeSocial = _mapper.Map<RedeSocial>(model);

                if (isEvento)
                {
                    RedeSocial.EventoId = id;
                    RedeSocial.PalestranteId = null;
                }else
                {
                    RedeSocial.EventoId = null;
                    RedeSocial.PalestranteId = id;                    
                }

                _redeSocialRepository.Add<RedeSocial>(RedeSocial);

                await _redeSocialRepository.SaveChangesAsync();                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialEventoByIdAsync(eventoId, redeSocialId);
                if (redeSocial == null)
                {
                    throw new Exception("RedeSocial por evento para deleção não encontrado");
                }

                _redeSocialRepository.Delete<RedeSocial>(redeSocial);
                return await _redeSocialRepository.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestrante(int PalestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialPalestranteByIdAsync(PalestranteId, redeSocialId);
                if (redeSocial == null)
                {
                    throw new Exception("RedeSocial por palestrante para deleção não encontrado");
                }

                _redeSocialRepository.Delete<RedeSocial>(redeSocial);
                return await _redeSocialRepository.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        public async Task<RedeSocialDto[]> GetAllByEventoIdAsync(int idEvento)
        {
            try
            {
                var RedeSocials = await _redeSocialRepository.GetAllByEventoIdAsync(idEvento);
                if (RedeSocials == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<RedeSocialDto[]>(RedeSocials);

                return resultad;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        

        public async Task<RedeSocialDto> GetRedeSocialEventoByIdAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var RedeSocials = await _redeSocialRepository.GetRedeSocialEventoByIdAsync(eventoId, redeSocialId);

                if (RedeSocials == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<RedeSocialDto>(RedeSocials);

                return resultad;
            }
            catch (System.Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialPalestranteByIdAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var RedeSocials = await _redeSocialRepository.GetRedeSocialPalestranteByIdAsync(palestranteId, redeSocialId);

                if (RedeSocials == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<RedeSocialDto>(RedeSocials);

                return resultad;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int PalestranteId)
        {
            try
            {
                var RedeSocials = await _redeSocialRepository.GetAllByPalestranteIdAsync(PalestranteId);
                if (RedeSocials == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<RedeSocialDto[]>(RedeSocials);

                return resultad;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                var RedeSocials = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);
                if (RedeSocials == null)
                {   
                    return null;
                }

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model, true);
                    }
                    else
                    {
                        var RedeSocial = RedeSocials.FirstOrDefault(RedeSocial => RedeSocial.Id == model.Id);

                        model.EventoId = eventoId;

                        _mapper.Map(model, RedeSocial);

                        _redeSocialRepository.Update<RedeSocial>(RedeSocial);

                        await _redeSocialRepository.SaveChangesAsync();
                    }
                }

                var rsRetorno = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);

                return _mapper.Map<RedeSocialDto[]>(rsRetorno);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public async Task<RedeSocialDto[]> SaveByPalestrante(int PalestranteId, RedeSocialDto[] models)
        {
            try
            {
                var RedeSocials = await _redeSocialRepository.GetAllByPalestranteIdAsync(PalestranteId);
                if (RedeSocials == null)
                {
                    return null;
                }

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(PalestranteId, model, false);
                    }
                    else
                    {
                        var RedeSocial = RedeSocials.FirstOrDefault(RedeSocial => RedeSocial.Id == model.Id);

                        model.PalestranteId = PalestranteId;

                        _mapper.Map(model, RedeSocial);

                        _redeSocialRepository.Update<RedeSocial>(RedeSocial);

                        await _redeSocialRepository.SaveChangesAsync();
                    }
                }

                var rsRetorno = await _redeSocialRepository.GetAllByPalestranteIdAsync(PalestranteId);

                return _mapper.Map<RedeSocialDto[]>(rsRetorno);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        
    }
}