using Compiladores.Semantico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Arbol.BaseNode
{
    public abstract class StatementNode
    {
         public Token token = null;
         public string archivo;
         public abstract void ValidateSemantic();
         public abstract string GenerarCodigo();
    }
}
