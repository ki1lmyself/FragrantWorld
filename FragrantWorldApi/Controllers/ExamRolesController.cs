using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FragrantWorldApi.Data;
using FragrantWorldApi.Models;

namespace FragrantWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamRolesController : ControllerBase
    {
        private readonly FragrantWorldStoreContext _context;

        public ExamRolesController(FragrantWorldStoreContext context)
        {
            _context = context;
        }

        // GET: api/ExamRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamRole>>> GetExamRoles()
        {
            return await _context.ExamRoles.ToListAsync();
        }

        // GET: api/ExamRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamRole>> GetExamRole(byte id)
        {
            var examRole = await _context.ExamRoles.FindAsync(id);

            if (examRole == null)
            {
                return NotFound();
            }

            return examRole;
        }

        // PUT: api/ExamRoles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamRole(byte id, ExamRole examRole)
        {
            if (id != examRole.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(examRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamRoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ExamRoles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExamRole>> PostExamRole(ExamRole examRole)
        {
            _context.ExamRoles.Add(examRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExamRole", new { id = examRole.RoleId }, examRole);
        }

        // DELETE: api/ExamRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamRole(byte id)
        {
            var examRole = await _context.ExamRoles.FindAsync(id);
            if (examRole == null)
            {
                return NotFound();
            }

            _context.ExamRoles.Remove(examRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamRoleExists(byte id)
        {
            return _context.ExamRoles.Any(e => e.RoleId == id);
        }
    }
}
