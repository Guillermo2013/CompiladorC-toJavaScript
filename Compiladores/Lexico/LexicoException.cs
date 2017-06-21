using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiladores
{
    class LexicoException : Exception
    {
        public LexicoException(){}
        public LexicoException(string mensaje) : base(mensaje) { }
    }
}
