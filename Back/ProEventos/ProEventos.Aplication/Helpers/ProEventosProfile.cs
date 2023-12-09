using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
// using ProEventos.API.Dtos;
using ProEventos.Aplication.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;

namespace ProEventos.Aplication.Helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<LoteDto, Lote>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<UserLoginDto, User>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<UserUpdateDto, User>().ReverseMap();
        }
    }
}