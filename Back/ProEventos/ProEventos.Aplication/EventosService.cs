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

        public async Task<EventoDto> AddEventos(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _geralRepository.Add<Evento>(evento);
                if (await _geralRepository.SaveChangesAsync())
                {
                    var retorno = await _eventosRepository.GetEventosByIdAsync(evento.Id, false);

                    return _mapper.Map<EventoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventosRepository.GetEventosByIdAsync(eventoId, false);
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

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetAllEventosAsync(includePalestrantes);
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

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
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

        public async Task<EventoDto> GetEventosByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetEventosByIdAsync(eventoId, includePalestrantes);
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

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventosRepository.GetEventosByIdAsync(eventoId, false);
                if (evento == null)
                {
                    return null;
                }

                model.Id = evento.Id;

                _mapper.Map(model, evento);

                _geralRepository.Update(evento);

                if (await _geralRepository.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventosRepository.GetEventosByIdAsync(model
                    .Id, false);

                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                
                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
            throw new NotImplementedException();
        }
    }
}