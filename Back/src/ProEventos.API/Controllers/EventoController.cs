using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public Evento[] _evento = new Evento[]
        {
            new Evento() {
                EventoId = 1,
                Tema = "Angular 11 e .Net 5",
                Local = "Sum Paulo",
                Lote = "1o. Lote",
                QtdePessoas = 25,
                DataEvento = DateTime.Now.AddDays(2).ToShortDateString(),
                ImagemURL = "c:/temp/foto.png"
            },
            new Evento() {
                EventoId = 2,
                Tema = "Nelson Piquet",
                Local = "Rio de Janeiro",
                Lote = "1o. Lote",
                QtdePessoas = 25,
                DataEvento = DateTime.Now.AddDays(5).ToShortDateString(),
                ImagemURL = "c:/temp/Piquet.png"
            }
        };

        public EventoController()
        {
                        
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _evento;
        }

        [HttpGet("{id}")]
        public IEnumerable<Evento> Get(int id)
        {
            return _evento.Where(evento => evento.EventoId == id);
        }

        [HttpPost]
        public string Post() 
        {
            return "Exemplo de Post";
        }

        [HttpPut("{id}")]
        public string Put(int id) 
        {
            return $"Exemplo de Put com parâmetro id: {id}";
        }
    }
}
