using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Aplication.Contracts;
using ProEventos.Domain.Models;
using ProEventos.Repository.Contratos;

namespace ProEventos.Aplication
{
    public class EventosService : IEventosService
    {
        private readonly IGeralRepository _geralRepository;
        private readonly IEventosRepository _eventosRepository;

        public EventosService(IGeralRepository geralRepository, IEventosRepository eventosRepository)
        {
            _eventosRepository = eventosRepository;
            _geralRepository = geralRepository;            
        }

        public async Task<Evento> AddEventos(Evento model)
        {
            try
            {
                _geralRepository.Add<Evento>(model);
                if (await _geralRepository.SaveChangesAsync())
                {
                    return await _eventosRepository.GetEventosByIdAsync(model.Id, false);
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

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetAllEventosAsync(includePalestrantes);
                if (eventos == null)
                {
                    return null;
                }
                return eventos;
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null)
                {
                    return null;
                }
                return eventos;
            }
            catch (System.Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventosByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetEventosByIdAsync(eventoId, includePalestrantes);
                if (eventos == null)
                {
                    return null;
                }
                return eventos;
                
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message) ;
            }
        }

        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {   
                var evento = await _eventosRepository.GetEventosByIdAsync(eventoId, false);
                if (evento == null)
                {
                    return null;
                }

                model.Id = evento.Id;

                _geralRepository.Update(model);
                if (await _geralRepository.SaveChangesAsync())
                {
                    return await _eventosRepository.GetEventosByIdAsync(model
                    .Id, false);
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