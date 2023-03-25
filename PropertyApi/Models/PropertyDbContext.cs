using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertyApi.Models;

namespace PropertyApi.Models
{
	public class PropertyDbContext : IdentityDbContext<ApplicationUser>
	{
		public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options)
		{
		}

        public DbSet<Property> Properties { get; set; }
    }
}
