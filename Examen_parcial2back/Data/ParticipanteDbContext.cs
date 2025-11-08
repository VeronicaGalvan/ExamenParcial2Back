using Examen_parcial2back.Models;
using Microsoft.EntityFrameworkCore;

namespace CongresoTICsAPI.Data
{
    public class ParticipanteDbContext : DbContext
    {
        public ParticipanteDbContext(DbContextOptions<ParticipanteDbContext> options)
            : base(options)
        {
        }

        public DbSet<Participante> Participantes { get; set; }
    }
}
