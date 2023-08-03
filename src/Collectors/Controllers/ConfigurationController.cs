using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Collectors.Model;
using Collectors.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Collectors.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : Controller
    {
        private readonly TargetDataContext _context;

        public ConfigurationController(TargetDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TargetConfiguration>>> GetTargets()
        {
            if(_context.Targets == null)
            {
                return Problem("Internal data error");
            }

            try
            {
                return Ok(_context.Targets.OrderBy(n => n.Name).ToList());
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpGet("{targetidd}")]
        public async Task<ActionResult<TargetConfiguration>> GetTargetConfig(string targetid)
        {
            try
            {
                var val = await _context.FindAsync<TargetConfiguration>(targetid);
                return Ok(val);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTarget(TargetConfiguration targetconfig)
        {
            try
            {
                await _context.AddAsync<TargetConfiguration>(targetconfig);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTarget(TargetConfiguration targetconfig)
        {
            try
            {
                _context.Update<TargetConfiguration>(targetconfig);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTarget(TargetConfiguration targetconfig)
        {
            try
            {
                _context.Remove<TargetConfiguration>(targetconfig);
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

