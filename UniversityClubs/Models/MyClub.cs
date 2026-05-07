using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityClubs.Models
{
    [MetadataType(typeof(MyDataAnnotation))]
    public partial class Club
    {
        public class MyDataAnnotation
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public int Category { get; set; }
            [Required]
            public string Description { get; set; }
            [Required]
            public string Mission { get; set; }
            [Required]
            public string Vision { get; set; }
            [Required]
            public string Goals { get; set; }
            [Required]
            [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must have 10 digits")]
            public string Phone { get; set; }
            [Required(ErrorMessage = "Room must be a number")]
            public int Room { get; set; }
            [Required]
            public string briefDesc { get; set; }
        }
    }
}