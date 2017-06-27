
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Arbol.BaseNode
{
    public abstract class ExpressionNode
    {
        public Token token = null;
        public string archivo;
        public abstract TiposBases ValidateSemantic();
        public abstract string GenerarCodigo();

        
    }
}
