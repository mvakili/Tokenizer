using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer
{
    public class Word
    {
        public string Normalized { get; set; }
        public bool IsStandard { get; set; }
        public List<string> Suggestions { get; set; }
        public List<Location> Locations { get; set; }
    }
}
