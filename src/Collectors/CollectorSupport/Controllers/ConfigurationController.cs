using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Collectors.Data;
using PalisaidMeta.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Collectors.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : Controller
    {
        private readonly CollectorDataContext _context;

        public ConfigurationController(CollectorDataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CollectorDataContext>>> GetTargets()
        {
            if(_context.Configs == null)
            {
                return Problem("Internal data error");
            }

            try
            {
                return Ok(_context.Configs.OrderBy(n => n.TargetName).ToList());
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("Reset/{configid}")]
        public async Task<ActionResult<string>> Reset(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid id");
            }

            var tconfig = _context.Configs.Where(i => i.EntityId == id).FirstOrDefault();
            if (tconfig == null)
            {
                return Problem("Config not found");
            }

            return Ok(tconfig.Reset());
        }

        [HttpGet("GetSingle/{targetid}")]
        public async Task<ActionResult<CollectorDataContext>> GetTargetConfig(string targetid)
        {
            try
            {
                var val = await _context.FindAsync<CollectorDataContext>(targetid);
                return Ok(val);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddNewTarget/{targetconfig}")]
        public async Task<IActionResult> AddTarget(CollectorConfig targetconfig)
        {
            try
            {
                await _context.AddAsync<CollectorConfig>(targetconfig);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateTarget/{targetconfig}")]
        public async Task<IActionResult> UpdateTarget(CollectorDataContext targetconfig)
        {
            try
            {
                _context.Update<CollectorDataContext>(targetconfig);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{targetconfig}")]
        public async Task<IActionResult> DeleteTarget(CollectorDataContext targetconfig)
        {
            try
            {
                _context.Remove<CollectorDataContext>(targetconfig);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

