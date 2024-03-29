﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicHub.Data.Models
{
    public class Writer
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3), MaxLength(20), Required]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+$")]
        public string Pseudonym  { get; set; }

        public ICollection<Song> Songs { get; set; } = new HashSet<Song>();
    }
}
