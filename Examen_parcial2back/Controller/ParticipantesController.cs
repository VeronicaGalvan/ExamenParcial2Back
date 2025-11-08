using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CongresoTICsAPI.Data;
using Examen_parcial2back.Models;

namespace Examen_parcial2back.Controller
{
    [Route("api")]
    [ApiController]
    public class ParticipantesController : ControllerBase
    {
        private readonly ParticipanteDbContext _context;

        public ParticipantesController(ParticipanteDbContext context)
        {
            _context = context;
        }

        // GET: api/Participantes
        [HttpGet("listado")]
        public async Task<ActionResult<IEnumerable<Participante>>> GetParticipantes()
        {
            return await _context.Participantes.ToListAsync();
        }

        // GET: /api/listado?q=texto
        [HttpGet("listado/buscar")]
        public async Task<ActionResult<IEnumerable<Participante>>> Buscar([FromQuery] string q)
        {
            if (string.IsNullOrEmpty(q))
                return await _context.Participantes.ToListAsync();

            return await _context.Participantes
                .Where(p => p.Nombre.Contains(q) || p.Apellidos.Contains(q))
                .ToListAsync();
        }

        // GET: api/Participantes/5
        [HttpGet("gafete/{id}")]
        public async Task<ActionResult<Participante>> GetParticipante(int id)
        {
            var participante = await _context.Participantes.FindAsync(id);

            if (participante == null)
            {
                return NotFound();
            }

            return participante;
        }

       
        // POST: api/Participantes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("registro")]
        public async Task<ActionResult<Participante>> PostParticipante(Participante participante)
        {
            _context.Participantes.Add(participante);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParticipante", new { id = participante.Id }, participante);
        }


    }
}
