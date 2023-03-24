using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PropertyApi.Models
{
    [Index(nameof(Address), IsUnique = true)]
    public class Property
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[Required]
		[MaxLength(100)]
        public string Address { get; set; }

		[Required]
        [MaxLength(10)]
        public string Postcode { get; set; }

		[Required]
        [Range(0, 100)]
        public int Bedrooms { get; set; }

		[Required]
        [Range(0, 100)]
        public int Bathrooms { get; set; }

        [Required]
        [RegularExpression("House|Flat|Bungalow")]
        public string PropertyType { get; set; }

        public Property(string address, string postcode, int bedrooms, int bathrooms, string propertyType)
        {
            Address = address;
            Postcode = postcode;
            Bedrooms = bedrooms;
            Bathrooms = bathrooms;
            PropertyType = propertyType;
        }
    }
}

