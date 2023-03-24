using System;
using Microsoft.EntityFrameworkCore;

namespace PropertyApi.Models
{
	public class PropertyDbContext : DbContext
	{
		public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options)
		{
		}

        public DbSet<Property> Properties { get; set; }
    }
}

