using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicHub.Data.Models
{
    public class SongPerformer
    {
        [ForeignKey("Song")]
        public int SongId { get; set; }

        [Required]
        public Song Song { get; set; }

        [ForeignKey("Performer")]

        [Required]
        public int PerformerId { get; set; }

        public Performer Performer { get; set; }


    }
}
