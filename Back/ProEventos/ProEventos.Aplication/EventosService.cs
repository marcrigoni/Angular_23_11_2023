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

namespace ProEventos.Aplication
{
    public class EventosService : IEventosService
    {
        private readonly IGeralRepository _geralRepository;
        private readonly IEventosRepository _eventosRepository;
        private readonly IMapper _mapper;

        public EventosService(IGeralRepository geralRepository, 
                                IEventosRepository eventosRepository,
                                IMapper mapper)
        {
            _mapper = mapper;
            _eventosRepository = eventosRepository;
            _geralRepository = geralRepository;            
        }

        public async Task<EventoDto> AddEventos(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _geralRepository.Add<Evento>(evento);
                if (await _geralRepository.SaveChangesAsync())
                {
                    var retorno = await _eventosRepository.GetEventosByIdAsync(userId, evento.Id, false);

                    return _mapper.Map<EventoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
            try
            {
                var evento = await _eventosRepository.GetEventosByIdAsync(userId, eventoId, false);
                if (evento == null)
                {
                    throw new Exception("Evento para deleção não encontrado");
                }

                evento.Id = evento.Id;

                _geralRepository.Delete(evento);
                return await _geralRepository.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetAllEventosAsync(userId, includePalestrantes);
                if (eventos == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<EventoDto[]>(eventos);


                return resultad;
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetAllEventosByTemaAsync(userId, tema, includePalestrantes);
                if (eventos == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<EventoDto[]>(eventos);

                return resultad;
            }
            catch (System.Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<EventoDto> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetEventosByIdAsync(userId,   eventoId, includePalestrantes);
                if (eventos == null)
                {
                    return null;
                }

                var resultad = _mapper.Map<EventoDto>(eventos);

                return resultad;
                
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message) ;
            }
        }


        public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventosRepository.GetEventosByIdAsync(userId, eventoId, false);
                if (evento == null)
                {
                    return null;
                }

                model.Id = evento.Id;
                model.UserId = userId;

                _mapper.Map(model, evento);

                _geralRepository.Update(evento);

                if (await _geralRepository.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventosRepository.GetEventosByIdAsync(userId, model.Id, false);

                    return _mapper.Map<EventoDto>(eventoRetorno);
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