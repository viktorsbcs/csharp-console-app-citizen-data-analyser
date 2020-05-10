using CommandLine;
using declared_persons_analyser.Utilities;
using System;
using System.Collections.Generic;
using System.Text;


namespace declared_persons_analyser.Models
{
    public class Options
    {
        private readonly string _default_url = GlobalDefaults.DEFAULT_URL;

        [Option("district", Required = true)]
        public int District { get; set; }

        [Option("limit", Default = 100 )]
        public int Limit { get; set; }

        [Option("group")]
        public string Group { get; set; }

        [Option("year" )]
        public int Year { get; set; }

        [Option("out")]
        public string Out { get; set; }

        [Option("source", Default = _default_url)]
        public string Source { get; set; }

        [Option("month")]
        public int Month { get; set; }

        [Option("day")]
        public int Day { get; set; }


    }
}
