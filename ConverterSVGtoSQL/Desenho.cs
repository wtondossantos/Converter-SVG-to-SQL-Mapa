using System;
using System.Collections.Generic;
using System.Text;

namespace ConverterSVGtoSQL
{
    public class Desenho
    {
        public string Id { get; set; }
        public string IdPai { get; set; }
        public string Nome { get; set; }
        public string Shape { get; set; }
        public int Index { get; set; }
    }
}
