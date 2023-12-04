using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Aplication.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public decimal Preco { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public int Qtde { get; set; }

        public int EventoId { get; set; }

        public EventoDto Evento { get; set; }
    }
}