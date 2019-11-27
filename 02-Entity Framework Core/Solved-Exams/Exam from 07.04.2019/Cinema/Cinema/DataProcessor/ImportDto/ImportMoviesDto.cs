using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.DataProcessor.ImportDto
{
    public class ImportMoviesDto
    {
        public string Title { get; set; }

        public string Genre { get; set; }

        public TimeSpan Duration { get; set; }

        public double Rating { get; set; }

        public string Director { get; set; }
    }
}
