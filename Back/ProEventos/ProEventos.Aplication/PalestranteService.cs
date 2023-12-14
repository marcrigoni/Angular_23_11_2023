using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Aplication.Contracts;
using ProEventos.Aplication.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;
using ProEventos.Repository.Contratos;
using ProEventos.Repository.Models;

namespace ProEventos.Aplication
{
    public class PalestranteService : IPalestranteService
    {                
        private readonly IPalestranteRepository _palestranteRepository;
        private readonly IMapper _mapper;

        public PalestranteService(
                                IPalestranteRepository palestranteRepository,
                                IMapper mapper)
        {
            _palestranteRepository = palestranteRepository;
            _mapper = mapper;                    
        }

        public async Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestranteRepository.Add<Palestrante>(palestrante);

                if (await _palestranteRepository.SaveChangesAsync())
                {
                    var retorno = await _palestranteRepository.GetPalestrantesByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        

        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includePalestrantes = false)
        {
            try
            {
                var Palestrantes = await _palestranteRepository.GetAllPalestrantesAsync( pageParams, includePalestrantes);

                if (Palestrantes == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<PageList<PalestranteDto>>(Palestrantes);

                resultad.CurrentPage = Palestrantes.CurrentPage;
                resultad.TotalPages = Palestrantes.TotalPages;
                resultad.PageSize = Palestrantes.PageSize;
                resultad.TotalCount = Palestrantes.TotalCount;                

                return resultad;
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }        
        
        public async Task<PalestranteDto> GetPalestrantesByUserIdAsync(int userId, bool includePalestrantes = false)
        {
            try
            {
                var Palestrantes = await _palestranteRepository.GetPalestrantesByUserIdAsync(userId, includePalestrantes);

                if (Palestrantes == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<PalestranteDto>(Palestrantes);

                return resultad;
                
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message) ;
            }
        }


        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var Palestrante = await _palestranteRepository.GetPalestrantesByUserIdAsync (userId, false);

                if (Palestrante == null)
                {
                    return null;
                }

                model.Id = Palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, Palestrante);

                _palestranteRepository.Update(Palestrante);

                if (await _palestranteRepository.SaveChangesAsync())
                {
                    var PalestranteRetorno = await _palestranteRepository.GetPalestrantesByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDto>(PalestranteRetorno);
                }
                
                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }
    }
}