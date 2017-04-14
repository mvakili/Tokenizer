using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer
{
    public struct Location
    {
        public string NormalizedValue { get; set; }
        public int Row { get; set; }
        public int CharNumber { get; set; }
        
    }
}
