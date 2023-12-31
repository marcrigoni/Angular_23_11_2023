using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Identity;

namespace ProEventos.Domain.Models
{    
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        public string Local { get; set; }

        public DateTime? DataEvento { get; set; }

        public string Tema { get; set; }

        public int QtdePessoas { get; set; }

        public string ImageURL { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public IEnumerable<Lote> Lotes { get; set; }

        public IEnumerable<RedeSocial> RedesSociais { get; set; }

        public IEnumerable<PalestranteEvento> PalestrantesEventos { get; set; }
    }
}