using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Aplication.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        public string Local { get; set; }

        public DateTime DataEvento { get; set; }

        [Required(ErrorMessage = "O Caompo {0} é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Intervalo permitido é de 3 a 50 chars!")]       
        public string Tema { get; set; }

        [Display(Name = "Qtde Pessoas")]
        [Range(1, 120000, ErrorMessage ="{0} não pode ser menor que 1 e maior que 120.000!")]
        public int QtdePessoas { get; set; }

        public string ImageURL { get; set; }

        [Required(ErrorMessage = "O caompo {0} é obrigatório!")]
        [Phone(ErrorMessage = "O campo {0} está com número inválido!")]
        public string Telefone { get; set; }

        [
            Required(ErrorMessage = "o campo {0} é obrigatório!"),
            Display(Name = "e-mail"),
            EmailAddress(ErrorMessage = "O campo {0} precisa ser um e-mail válido!")]
        public string Email { get; set; }      

        public IEnumerable<LoteDto> Lotes { get; set; }

        public IEnumerable<RedeSocialDto> RedeSociais { get; set; }

        public IEnumerable<PalestranteDto> Palestrantes { get; set; }

    }
}