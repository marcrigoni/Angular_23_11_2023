using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;

namespace ProEventos.Repository.Context
{
    public class ProEventosContext : DbContext
    {
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options)
        {

        }
        public DbSet<Evento> Eventos { get; set; }

        public DbSet<Lote> Lotes { get; set; }

        public DbSet<Palestrante> Palestrantes { get; set; }

        public DbSet<PalestranteEvento> PalestranteEventos { get; set; }

        public DbSet<RedeSocial> RedesSociais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PalestranteEvento>().HasKey(pe =>
                new { pe.EventoId, pe.PalestranteId }
            );

            modelBuilder.Entity<Evento>()
                .HasMany(e => e.RedesSociais)
                .WithOne(rs => rs.Evento)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Palestrante>()
            .HasMany(e => e.RedesSociais)
            .WithOne(rs => rs.Palestrante)
            .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            

            // services.AddDbContext<ProEventosContext>(
            //     context => context.UseSqlite(Configuration.GetConnectionString("Default"))
            // );

         
        }
    }
}
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using ProEventos.Domain.Models;

// namespace ProEventos.Repository
// {
//     public class ProEventosContext : DbContext
//     {

//         public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) { }

//         public DbSet<Evento> Eventos { get; set; }

//         public DbSet<Lote> Lotes { get; set; }

//         public DbSet<Palestrante> Palestrantes { get; set; }

//         public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }

//         public DbSet<RedeSocial> RedesSociais  { get; set; }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);

//             modelBuilder.Entity<PalestranteEvento>().HasKey(PE => new { PE.EventoId, PE.PalestranteId });
//         }
//     }
// }