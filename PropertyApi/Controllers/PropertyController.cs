using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PropertyApi.Models;

namespace PropertyApi.Controllers
{
	[ApiController]
	[Route("properties")]
	public class PropertyController : ControllerBase
	{
		private readonly PropertyDbContext _context;

		public PropertyController(PropertyDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public ActionResult<List<Property>> GetProperties()
		{
			return _context.Properties.ToList();
		}

		[HttpGet("{id}")]
		public ActionResult<Property> GetProperty(int id)
		{
			var property = _context.Properties.Find(id);

			if (property == null)
			{
				return NotFound();
			}

			return property;
		}

		[HttpGet("postcode/{postcode}")]
		public ActionResult<List<Property>> GetPropertiesByPostcode(string postcode)
		{
            var properties = _context.Properties.Where(p => p.Postcode.StartsWith(postcode)).ToList();

			if (properties.Count == 0)
			{
                return NotFound();
            }
            return Ok(properties);
        }

		[HttpPost]
		public ActionResult<Property> CreateProperty(Property property)
		{
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			try
			{
                _context.Properties.Add(property);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
            }
			catch (DbUpdateException ex)
			{
                if (ex.InnerException is MySqlException mysqlEx && mysqlEx.Number == 1062)
                {
                    ModelState.AddModelError("Address", "An existing property has the same address.");
                    return BadRequest(ModelState);
                }
                else
                {
                    throw;
                }
            }
		}

		[HttpPut("{id}")]
		public IActionResult UpdateProperty(int id, Property property)
		{
			if (id != property.Id)
			{
				return BadRequest();
			}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(property).State = EntityState.Modified;

			try
			{
				_context.SaveChanges();
			}
			catch(DbUpdateConcurrencyException)
			{
				if (!PropertyExists(id))
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

		[HttpDelete("{id}")]
		public IActionResult DeleteProperty(int id)
		{
			var property = _context.Properties.Find(id);
			if (property == null)
			{
				return NotFound();
			}

			_context.Properties.Remove(property);
			_context.SaveChanges();

			return NoContent();
		}

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(p => p.Id == id);
        }
    }
}

